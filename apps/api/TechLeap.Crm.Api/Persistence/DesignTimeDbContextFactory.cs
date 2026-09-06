using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TechLeap.Crm.BuildingBlocks.Persistence;

namespace TechLeap.Crm.Api.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlatformDbContext>
{
    public PlatformDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? "Host=localhost;Port=5432;Database=tech_leap_crm;Username=tech_leap;Password=tech_leap_dev";

        var options = new DbContextOptionsBuilder<PlatformDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName))
            .Options;

        return new PlatformDbContext(options);
    }
}
