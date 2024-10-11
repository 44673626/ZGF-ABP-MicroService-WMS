using WMS.BaseService.Logs;
using WMS.BaseService.Logs;
using WMS.BaseService.Samples;
using WMS.BaseService.Samples.DataDictionarys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace WMS.BaseService.EntityFrameworkCore;

[ConnectionStringName(ABPVNextDbProperties.ConnectionStringName)]
public class ABPVNextDbContext : AbpDbContext<ABPVNextDbContext>, IABPVNextDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */
    public DbSet<AbpLogInfo> AbpLogInfos { get; set; }//自定义记录日志
    public DbSet<Bom> Boms { get; set; }//用于测试文件导入、导出

    public DbSet<DataDictionary> DataDictionaries { get; set; }//测试用于缓存实例
    /// <summary>
    /// 租户
    /// </summary>
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    public ABPVNextDbContext(DbContextOptions<ABPVNextDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureTenantManagement();//底层封装租户2张表
        builder.ConfigureABPVNext();
       
    }
}
