using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace FileStorage.HttpApi.Client
{
    [DependsOn(
    typeof(FileStorageApplicationContractsModule),
    typeof(AbpHttpClientModule))]
    public class FileStorageHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "FileStorage";
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(FileStorageApplicationContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<FileStorageHttpApiClientModule>();
            });

        }
    }
}
