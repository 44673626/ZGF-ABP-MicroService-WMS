using WMS.BaseService.Logs;
using WMS.BaseService.Logs;
using WMS.BaseService.Samples;
using WMS.BaseService.Samples.DataDictionarys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace WMS.BaseService.EntityFrameworkCore;

public static class ABPVNextDbContextModelCreatingExtensions
{
    public static void ConfigureABPVNext(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureBom();//测试用

        builder.Entity<AbpLogInfo>(b =>
        {
            b.ToTable("AbpLogs");

            b.ConfigureByConvention();
        });



        builder.Entity<DataDictionary>(b =>
        {
            b.ToTable("base_dict");

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(BaseServiceConsts.MaxNameLength);
            b.Property(x => x.Description).HasMaxLength(BaseServiceConsts.MaxNotesLength);
            b.Property(x => x.IsDeleted).HasDefaultValue(false);

            b.HasIndex(q => q.Name);
        });

        //builder.Entity<DataDictionaryDetail>(b =>
        //{
        //    b.ToTable("base_dict_details");

        //    b.ConfigureByConvention();

        //    b.Property(x => x.Label).IsRequired().HasMaxLength(BaseServiceConsts.MaxNameLength);
        //    b.Property(x => x.Value).IsRequired().HasMaxLength(BaseServiceConsts.MaxNotesLength);
        //    b.Property(x => x.IsDeleted).HasDefaultValue(false);

        //    b.HasIndex(q => q.Pid);
        //});

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(ABPVNextDbProperties.DbTablePrefix + "Questions", ABPVNextDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
    }

    private static void ConfigureBom(this ModelBuilder builder)
    {
        builder.Entity<Bom>(b =>
        {

            b.ToTable("abp_bom");
            b.ConfigureByConvention();

            b.Property(x => x.Year).HasMaxLength(200);
            b.Property(x => x.Period).HasMaxLength(256);
            b.Property(x => x.Factory).HasMaxLength(256);
            b.Property(x => x.Version).HasMaxLength(256);
            b.Property(x => x.ParentItemCode).IsRequired().HasMaxLength(256);
            b.Property(x => x.ChildItemCode).IsRequired().HasMaxLength(256);
            b.Property(x => x.Qty).IsRequired();
            b.Property(x => x.OperateProcess);
            b.Property(x => x.ScrapPercent);
            b.Property(x => x.BomType).HasMaxLength(256);
            b.Property(x => x.EffectiveTime);
            b.Property(x => x.ExpireTime);
            b.Property(x => x.IssuePosition);
            b.Property(x => x.BomLevel);


        });
    }
}
