using Localization.Resources.AbpUi;
using WMS.BaseService.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace WMS.BaseService;

[DependsOn(
    typeof(WMSBaseApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class WMSBaseHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(WMSBaseHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WMSBaseResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
