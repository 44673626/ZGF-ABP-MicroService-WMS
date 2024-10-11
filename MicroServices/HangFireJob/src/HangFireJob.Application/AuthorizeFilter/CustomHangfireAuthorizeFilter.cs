using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.AuthorizeFilter
{
    public class CustomHangfireAuthorizeFilter : IDashboardAuthorizationFilter
    {
        //这里写自定义规则//默认是只能内网访问。 需要重写这个方法。注入规则
        public bool Authorize([NotNull] DashboardContext context)
        {
            if (context.Request.LocalIpAddress.Equals("127.0.0.1") || context.Request.LocalIpAddress.Equals("::1"))
                return true;
            else
                return false;
        }
    }
}
