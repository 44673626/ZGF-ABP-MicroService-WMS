using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using System;
using HangFireJob.Services.BackGroudJobs;
using HangFireJob.Samples.ExportJob;

namespace HangFireJob;
[DependsOn(
    typeof(HangFireJobDomainModule),
    typeof(HangFireJobApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class HangFireJobApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<HangFireJobApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<HangFireJobApplicationModule>(validate: false);
        });

        //注册后台执行任务的服务
        context.Services.AddTransient<HExportJob>();
   
    }
}
