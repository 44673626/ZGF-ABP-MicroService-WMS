using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace ABP.Business;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ABPVNextDomainSharedModule)
)]
public class ABPVNextDomainModule : AbpModule
{

}
