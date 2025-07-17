using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WMS.BaseService.EntityFrameworkCore;

public class ABPVNextHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ABPVNextHttpApiHostMigrationsDbContext>
{
    public ABPVNextHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        BaseEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ABPVNextHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString(WMSBaseDbProperties.ConnectionStringName));

        return new ABPVNextHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
