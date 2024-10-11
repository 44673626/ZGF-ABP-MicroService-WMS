using HangFireJob.Settings;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HangFireJob.EntityFrameworkCore;

[ConnectionStringName(HangFireJobDbProperties.ConnectionStringName)]
public class HangFireJobDbContext : AbpDbContext<HangFireJobDbContext>, IHangFireJobDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */
    public DbSet<HttpJobDescriptor> HttpJobDescriptors { get; set; }


    public HangFireJobDbContext(DbContextOptions<HangFireJobDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureHangFireJob();
    }
}
