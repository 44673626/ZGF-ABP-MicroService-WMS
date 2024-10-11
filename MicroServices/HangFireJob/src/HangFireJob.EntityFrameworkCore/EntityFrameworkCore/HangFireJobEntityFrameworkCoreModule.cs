using HangFireJob.Dapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace HangFireJob.EntityFrameworkCore;

[DependsOn(
    typeof(HangFireJobDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpDapperModule)//依赖关系的先后顺序 AbpDapperModule 依赖应该在 EF Core依赖之后
)]
public class HangFireJobEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<HangFireJobDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            //Configure<AbpDbContextOptions>(options =>
            //{
            //    options.UseSqlServer();
            //});
            //注册仓储等
            context.Services.AddAbpDbContext<HangFireJobDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        });
        //注册dapper作用域,注入到IOC容器
        context.Services.AddScoped<DapperDbContext>();
    }
}
