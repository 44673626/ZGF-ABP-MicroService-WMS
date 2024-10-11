using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using WMS.BaseService.CommonManagement.UploadBlobFiles;

namespace WMS.BaseService;

[DependsOn(
    typeof(ABPVNextDomainModule),
    typeof(ABPVNextApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class ABPVNextApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ABPVNextApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ABPVNextApplicationModule>(validate: false);//true开启后，全字段匹配了，目前不需要全匹配
        });
    }
}
