using WMS.BaseService.Logs;
using WMS.BaseService.Logs;
using WMS.BaseService.Samples;
using WMS.BaseService.Samples.DataDictionarys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using WMS.BaseService.BusinessEntity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace WMS.BaseService.EntityFrameworkCore;

public static class WMSBaseDbContextModelCreatingExtensions
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

        #region >业务配置<
        builder.Entity<UnitConversion>(b =>
        {
            b.ToTable(TableName<UnitConversion>());
            b.ConfigureByConvention();
        });
        //仓库信息
        builder.Entity<Warehouse>(b =>
        {
            b.ToTable(TableName<Warehouse>());
            b.ConfigureByConvention();
        });
        #endregion
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

    private static string TableName<T>()
    {
        return typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? nameof(T);
    }
}
