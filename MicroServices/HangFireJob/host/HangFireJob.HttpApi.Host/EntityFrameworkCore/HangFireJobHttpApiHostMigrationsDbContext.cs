using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace HangFireJob.EntityFrameworkCore;

public class HangFireJobHttpApiHostMigrationsDbContext : AbpDbContext<HangFireJobHttpApiHostMigrationsDbContext>
{
    public HangFireJobHttpApiHostMigrationsDbContext(DbContextOptions<HangFireJobHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureHangFireJob();
    }
}
