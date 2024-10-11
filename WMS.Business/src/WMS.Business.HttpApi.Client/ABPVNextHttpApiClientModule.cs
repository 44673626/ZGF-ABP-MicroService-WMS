using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace WMS.Business;

[DependsOn(
    typeof(ABPVNextApplicationContractsModule),//包含应用服务接口
    typeof(AbpHttpClientModule))]//用来创建客户端代理
public class ABPVNextHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //创建动态客户端代理
        context.Services.AddHttpClientProxies(
            typeof(ABPVNextApplicationContractsModule).Assembly,
            ABPVNextRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ABPVNextHttpApiClientModule>();
        });

    }
}
