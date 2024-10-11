using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace HangFireJob;

[DependsOn(
    typeof(HangFireJobDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class HangFireJobApplicationContractsModule : AbpModule
{

}
