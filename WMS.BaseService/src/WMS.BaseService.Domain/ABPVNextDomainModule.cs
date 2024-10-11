using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace WMS.BaseService;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ABPVNextDomainSharedModule)
)]
public class ABPVNextDomainModule : AbpModule
{

}
