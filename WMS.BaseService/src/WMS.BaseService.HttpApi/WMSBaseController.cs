using WMS.BaseService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace WMS.BaseService;

public abstract class WMSBaseController : AbpControllerBase
{
    protected WMSBaseController()
    {
        LocalizationResource = typeof(WMSBaseResource);
    }
}
