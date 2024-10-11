using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HangFireJob.EntityFrameworkCore;

public class HangFireJobHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<HangFireJobHttpApiHostMigrationsDbContext>
{
    public HangFireJobHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<HangFireJobHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("HangFireJob"));

        return new HangFireJobHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
