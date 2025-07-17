using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace WMS.BaseService;

[DependsOn(
    typeof(WMSBaseApplicationContractsModule),//包含应用服务接口
    typeof(AbpHttpClientModule))]//用来创建客户端代理
public class WMSBaseHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //创建动态客户端代理
        context.Services.AddHttpClientProxies(
            typeof(WMSBaseApplicationContractsModule).Assembly,
            WMSBaseRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WMSBaseHttpApiClientModule>();
        });

    }
}
