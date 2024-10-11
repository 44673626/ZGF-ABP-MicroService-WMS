using BaseService.Systems.MessageManagement;
using System;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace BaseService
{
    [DependsOn(
        typeof(BaseServiceDomainModule),
        typeof(BaseServiceApplicationContractsModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpIdentityApplicationModule),
       // typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpAutoMapperModule)
    )]
    public class BaseServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<BaseServiceApplicationAutoMapperProfile>();
            });

            //#region SignalR配置

            //Configure<AbpSignalROptions>(options =>
            //{
            //    options.Hubs.AddOrUpdate(
            //        typeof(MessageHub), //Hub type
            //        config => //Additional configuration
            //        {
            //            config.RoutePattern = "/api/v1/sys/signalr-hubs"; //override the default route
            //            config.ConfigureActions.Add(hubOptions =>
            //            {
            //                //Additional options
            //                hubOptions.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            //            });
            //        }
            //    );
            //});
            //#endregion
        }
    }
}
