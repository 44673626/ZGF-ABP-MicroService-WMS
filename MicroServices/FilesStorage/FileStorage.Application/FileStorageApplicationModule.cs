using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Modularity;

namespace FileStorage
{
    //[DependsOn(
    //    typeof(FileStorageApplicationContractsModule),
    //    //        typeof(AbpHttpClientIdentityModelModule),
    //    //typeof(AbpIdentityHttpApiClientModule),
    //    typeof(AbpAutoMapperModule),
    //    //typeof(AbpAspNetCoreMvcModule)
    //    typeof(AbpAutoMapperModule)
    //)]
    [DependsOn(
       typeof(FileStorageDomainModule),
       typeof(FileStorageApplicationContractsModule),
      // typeof(AbpHttpClientIdentityModelWebModule),//用于UseCurrentAccessToken设置为true
       typeof(AbpAutoMapperModule)
   )]
    public class FileStorageApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FileStorageApplicationModule>();
            });
        }
    }
}
