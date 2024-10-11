using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace WMS.Business;

[DependsOn(
    typeof(ABPVNextDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ABPVNextApplicationContractsModule : AbpModule
{

}
