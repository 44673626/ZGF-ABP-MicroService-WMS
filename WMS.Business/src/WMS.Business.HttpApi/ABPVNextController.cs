using WMS.Business.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WMS.Business;

public abstract class ABPVNextController : AbpControllerBase
{
    protected ABPVNextController()
    {
        LocalizationResource = typeof(ABPVNextResource);
    }
}
