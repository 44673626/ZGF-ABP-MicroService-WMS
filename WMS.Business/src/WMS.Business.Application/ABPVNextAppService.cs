using WMS.Business.Localization;
using Volo.Abp.Application.Services;

namespace WMS.Business;

public abstract class ABPVNextAppService : ApplicationService
{
    protected ABPVNextAppService()
    {
        LocalizationResource = typeof(ABPVNextResource);
        ObjectMapperContext = typeof(ABPVNextApplicationModule);
    }
}
