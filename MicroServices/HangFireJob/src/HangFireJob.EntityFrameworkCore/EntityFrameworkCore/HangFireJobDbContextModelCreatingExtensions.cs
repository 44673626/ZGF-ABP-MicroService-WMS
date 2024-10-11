using HangFireJob.Settings;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace HangFireJob.EntityFrameworkCore;

public static class HangFireJobDbContextModelCreatingExtensions
{
    public static void ConfigureHangFireJob(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        //任务调度表
        builder.Entity<HttpJobDescriptor>(b =>
        {
            b.ToTable("Hangfire_HttpJob");

            b.ConfigureByConvention();

            b.Property(x => x.HttpUrl).IsRequired().HasMaxLength(512);
            b.Property(x => x.JobName).IsRequired().HasMaxLength(512);
            b.Property(x => x.JobType).IsRequired().HasMaxLength(512);
            b.Property(x => x.HttpMethod).IsRequired();
            b.Property(x => x.JobParameter).IsRequired(false).HasMaxLength(512);
            b.Property(x => x.Cron).IsRequired(false).HasMaxLength(256);
            b.Property(x => x.Remark).IsRequired(false).HasMaxLength(512);
            b.Property(x => x.StateName).IsRequired(false).HasMaxLength(128);
            b.Property(x => x.LastJobState).IsRequired(false).HasMaxLength(128);
        });


    }
}
