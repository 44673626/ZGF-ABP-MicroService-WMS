using Localization.Resources.AbpUi;
using HangFireJob.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace HangFireJob;

[DependsOn(
    typeof(HangFireJobApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class HangFireJobHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(HangFireJobHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<HangFireJobResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
