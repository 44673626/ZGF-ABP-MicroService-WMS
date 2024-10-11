using Localization.Resources.AbpUi;
using ABP.Business.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ABP.Business;

[DependsOn(
    typeof(ABPVNextApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class ABPVNextHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ABPVNextHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ABPVNextResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
