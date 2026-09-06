using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechLeap.Crm.Api.Web;
using TechLeap.Crm.BuildingBlocks.Persistence;
using TechLeap.Crm.BuildingBlocks.Web;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionStrings:Default is required.");
var auth0Authority = configuration["Auth0:Authority"];
var auth0Audience = configuration["Auth0:Audience"];
var jwtIssuer = configuration["Jwt:Issuer"] ?? "tech-leap-crm-local";
var jwtAudience = configuration["Jwt:Audience"] ?? "tech-leap-crm-api";
var jwtSigningKey = configuration["Jwt:SigningKey"]
    ?? throw new InvalidOperationException("Jwt:SigningKey is required when Auth0 is not configured.");

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["correlationId"] = context.HttpContext.Items[CorrelationIdMiddleware.HeaderName]?.ToString();
    };
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<PlatformDbContext>(options =>
    options.UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly(typeof(Program).Assembly.FullName)));
builder.Services.AddHealthChecks().AddCheck<PostgresHealthCheck>("postgres", tags: ["ready"]);
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        var origins = configuration.GetSection("AllowedOrigins").GetChildren().Select(item => item.Value).OfType<string>().ToArray();
        if (origins.Length == 0)
        {
            origins = ["http://localhost:3000"];
        }

        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        if (!string.IsNullOrWhiteSpace(auth0Authority) && !string.IsNullOrWhiteSpace(auth0Audience))
        {
            options.Authority = auth0Authority;
            options.Audience = auth0Audience;
            options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        }
        else
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        }
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseExceptionHandler();
app.UseCors("frontend");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapPost("/api/v1/dev/token", (DevTokenRequest request, IConfiguration currentConfiguration) =>
    {
        var issuer = currentConfiguration["Jwt:Issuer"] ?? jwtIssuer;
        var audience = currentConfiguration["Jwt:Audience"] ?? jwtAudience;
        var signingKey = currentConfiguration["Jwt:SigningKey"] ?? jwtSigningKey;
        var now = DateTime.UtcNow;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Subject),
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.Role, request.Role)
        };
        var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer, audience, claims, now, now.AddHours(1), credentials);
        return Results.Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), tokenType = "Bearer", expiresIn = 3600 });
    }).AllowAnonymous().ExcludeFromDescription();
}

app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });
app.MapGet("/api/v1/diagnostics", (HttpContext httpContext, IHostEnvironment environment, IConfiguration currentConfiguration) =>
    Results.Ok(new
    {
        service = "tech-leap-crm-api",
        version = currentConfiguration["ServiceInfo:Version"] ?? "0.1.0",
        environment = environment.EnvironmentName,
        utc = DateTime.UtcNow,
        traceId = httpContext.TraceIdentifier,
        correlationId = httpContext.Items[CorrelationIdMiddleware.HeaderName]?.ToString()
    })).RequireAuthorization();

app.Run();

public sealed record DevTokenRequest(string Subject = "local-user", string Email = "local@tech-leap.test", string Role = "Developer");

public partial class Program;
