using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace WMS.BaseService;

[DependsOn(
    typeof(WMSBaseDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class WMSBaseApplicationContractsModule : AbpModule
{

}
