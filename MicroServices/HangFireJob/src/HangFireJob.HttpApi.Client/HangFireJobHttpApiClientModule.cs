using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace HangFireJob;

[DependsOn(
    typeof(HangFireJobApplicationContractsModule),//用来创建客户端代理
    typeof(AbpHttpClientModule)//包含应用服务接口
    )]
public class HangFireJobHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //创建动态客户端代理
        context.Services.AddHttpClientProxies(
            typeof(HangFireJobApplicationContractsModule).Assembly,
            HangFireJobRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<HangFireJobHttpApiClientModule>();
        });

    }
}
