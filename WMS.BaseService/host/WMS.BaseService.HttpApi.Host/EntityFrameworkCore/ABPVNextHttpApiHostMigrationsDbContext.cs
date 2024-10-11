using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace WMS.BaseService.EntityFrameworkCore;

public class ABPVNextHttpApiHostMigrationsDbContext : AbpDbContext<ABPVNextHttpApiHostMigrationsDbContext>
{
    public ABPVNextHttpApiHostMigrationsDbContext(DbContextOptions<ABPVNextHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureABPVNext();
    }
}
