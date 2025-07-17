using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using WMS.BaseService.CommonManagement.UploadBlobFiles;

namespace WMS.BaseService;

[DependsOn(
    typeof(WMSBaseDomainModule),
    typeof(WMSBaseApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class WMSBaseApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<WMSBaseApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WMSBaseApplicationModule>(validate: false);//true开启后，全字段匹配了，目前不需要全匹配
        });
    }
}
