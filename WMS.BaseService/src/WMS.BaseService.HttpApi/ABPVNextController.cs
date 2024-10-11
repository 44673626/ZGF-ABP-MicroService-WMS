using WMS.BaseService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WMS.BaseService;

public abstract class ABPVNextController : AbpControllerBase
{
    protected ABPVNextController()
    {
        LocalizationResource = typeof(ABPVNextResource);
    }
}
