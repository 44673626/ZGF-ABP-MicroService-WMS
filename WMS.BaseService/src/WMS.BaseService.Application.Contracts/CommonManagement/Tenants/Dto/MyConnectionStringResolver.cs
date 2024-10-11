using WMS.BaseService.CommonManagement.UploadBlobFiles.comm;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace WMS.BaseService.CommonManagement.Tenants.Dto
{
    //public class MyConnectionStringResolver : DefaultConnectionStringResolver
    //{
    //    private readonly IConfigurationRoot _appConfiguration;
    //    public Abp.Runtime.Session.IAbpSession AbpSession { get; set; }
    //    private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
    //    private readonly MultiTenancy.TenantManager _tenantManager;


    //    public MyConnectionStringResolver(IAbpStartupConfiguration configuration,
    //        IHostingEnvironment hostingEnvironment,
    //        ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
    //        MultiTenancy.TenantManager tenantManager
    //        )
    //        : base(configuration)
    //    {
    //        _appConfiguration =
    //            AppConfigurations.Get(hostingEnvironment.ContentRootPath, hostingEnvironment.EnvironmentName);
    //        _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
    //        AbpSession = Abp.Runtime.Session.NullAbpSession.Instance;
    //        _tenantManager = tenantManager;
    //    }

    //    public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
    //    {
    //        if (args["DbContextConcreteType"] as Type == typeof(DbSecondServer.DbSecondEntityFrameworkCore.Models.DbSecondContext))
    //        {
    //            var tenant = _tenantManager.GetById(GetCurrentTenantId().GetValueOrDefault());
    //            var str = "Server=" + tenant.DatabaseServer + "; Database=" + tenant.DatabaseName + "; Uid=" + tenant.DatabaseUid + ";Password=" + tenant.DatabasePassword + ";";  //租户类里加了链接字符串的4参数
    //            return str;
    //        }

    //        return base.GetNameOrConnectionString(args);
    //    }

    //    protected virtual int? GetCurrentTenantId()
    //    {
    //        return _currentUnitOfWorkProvider.Current != null
    //            ? _currentUnitOfWorkProvider.Current.GetTenantId()
    //            : AbpSession.TenantId;
    //    }
    //}

}
