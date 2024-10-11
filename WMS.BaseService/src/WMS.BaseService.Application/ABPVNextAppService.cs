using WMS.BaseService.Localization;
using Volo.Abp.Application.Services;

namespace WMS.BaseService;

public abstract class ABPVNextAppService : ApplicationService
{
    protected ABPVNextAppService()
    {
        LocalizationResource = typeof(ABPVNextResource);
        ObjectMapperContext = typeof(ABPVNextApplicationModule);
    }
}
