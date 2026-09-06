using Microsoft.EntityFrameworkCore;
using TechLeap.Crm.BuildingBlocks.Persistence;
using TechLeap.Crm.Worker;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionStrings:Default is required.");

builder.Services.AddDbContext<PlatformDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddHostedService<OutboxPublisherWorker>();

await builder.Build().RunAsync();
