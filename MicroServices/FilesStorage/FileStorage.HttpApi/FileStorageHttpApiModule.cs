// 闻荫智慧工厂管理套件
//  Copyright (c) 闻荫科技 www.ccwin-in.com

using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace FileStorage
{
    [DependsOn(
        typeof(FileStorageApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class FileStorageHttpApiModule : AbpModule
    {
        //public override void PreConfigureServices(ServiceConfigurationContext context)
        //{
        //    PreConfigure<IMvcBuilder>(mvcBuilder =>
        //    {
        //        mvcBuilder.AddApplicationPartIfNotExists(typeof(FileStorageHttpApiModule).Assembly);
        //    });
        //}
    }
}
