using WMS.BaseService.Dappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using Volo.Abp.Dapper;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using WMS.BaseService.Samples.DataDictionarys;
using WMS.BaseService.CommonManagement.UploadBlobFiles;
using Volo.Abp.BlobStoring;

namespace WMS.BaseService.EntityFrameworkCore;

[DependsOn(
    typeof(ABPVNextDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),//tenant
     typeof(AbpDapperModule) //引入dapper,轻量化的ORM数据库提供者
)]
public class ABPVNextEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
 
        context.Services.AddAbpDbContext<ABPVNextDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            //开启后，非聚合根实体也支持IRepository注入
            options.AddDefaultRepositories(includeAllEntities: true);
            //注册业务服务
            context.Services.AddTransient<IDictionaryRepository, DictionaryEfCoreRepository>();

        });

        //注册dapper作用域,注入到IOC容器
        context.Services.AddScoped<DapperDbContext>();
    }
 
}
