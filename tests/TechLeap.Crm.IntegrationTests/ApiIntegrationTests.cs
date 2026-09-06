using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechLeap.Crm.BuildingBlocks.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

namespace TechLeap.Crm.IntegrationTests;

public sealed class ApiIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("tech_leap_crm_test")
        .WithUsername("tech_leap")
        .WithPassword("test_password")
        .Build();

    private WebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;

    public async Task InitializeAsync()
    {
        await postgres.StartAsync();
        factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("ConnectionStrings:Default", postgres.GetConnectionString());
            builder.UseSetting("Jwt:SigningKey", "integration-test-signing-key-123456789");
        });
        client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PlatformDbContext>();
        await db.Database.MigrateAsync();
        Assert.Empty(await db.OutboxMessages.ToListAsync());
    }

    public async Task DisposeAsync()
    {
        client.Dispose();
        await factory.DisposeAsync();
        await postgres.DisposeAsync();
    }

    [Fact]
    public async Task LiveHealthCheckDoesNotRequireDatabase()
    {
        var response = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ReadyHealthCheckConfirmsPostgresConnection()
    {
        var response = await client.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DiagnosticsRequiresJwtAndReturnsCorrelation()
    {
        var unauthorized = await client.GetAsync("/api/v1/diagnostics");
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode);

        var tokenResponse = await client.PostAsJsonAsync("/api/v1/dev/token", new { subject = "integration-user" });
        tokenResponse.EnsureSuccessStatusCode();
        var token = (await tokenResponse.Content.ReadFromJsonAsync<DevTokenResponse>())!.AccessToken;

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/diagnostics");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Add("X-Correlation-Id", "integration-correlation");
        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("integration-correlation", response.Headers.GetValues("X-Correlation-Id").Single());
        var body = await response.Content.ReadFromJsonAsync<DiagnosticsResponse>();
        Assert.Equal("integration-correlation", body!.CorrelationId);
    }

    private sealed record DevTokenResponse(string AccessToken);

    private sealed record DiagnosticsResponse(string? CorrelationId);
}
