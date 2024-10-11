using Comm.ConsulConfig;
using Comm.ConsulConfig.Registry.Options;
using FileStorage.Configurations;
using Hangfire;
using Hangfire.SqlServer;
using HangFireJob.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
//using HangFireJob.MultiTenancy;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BackgroundWorkers.Hangfire;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.VirtualFileSystem;
namespace HangFireJob;
[DependsOn(
    typeof(HangFireJobApplicationModule),
    typeof(HangFireJobEntityFrameworkCoreModule),
    typeof(HangFireJobHttpApiModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpBackgroundJobsHangfireModule),
    typeof(AbpBackgroundWorkersHangfireModule),
    typeof(CommConsulModule), //引入consul
    typeof(AbpSwashbuckleModule)
    )]
public class HangFireJobHttpApiHostModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
        ConfigConsul(context, configuration);//集成consul

        //post方式防伪攻击
        context.Services.Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });
        //Configure<AbpMultiTenancyOptions>(options =>
        //{
        //    options.IsEnabled = MultiTenancyConsts.IsEnabled;
        //});

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<HangFireJobDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HangFireJob.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<HangFireJobDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HangFireJob.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<HangFireJobApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HangFireJob.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<HangFireJobApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HangFireJob.Application", Path.DirectorySeparatorChar)));
            });
        }

        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                {"HangFireJob", "HangFireJob API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "HangFireJob API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

        });

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "HangFireJob";
            });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "HangFireJob:";
        });

        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("HangFireJob");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "HangFireJob-Protection-Keys");
        }

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray() ?? Array.Empty<string>()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        //注册中间件hangfire
        ConfigureHangfire(context, configuration);


    }
    /// <summary>
    /// consul服务注册
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    private void ConfigConsul(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // consul注册
        Configure<ServiceRegistryOptions>(options =>
        {
            options.ServiceId = Guid.NewGuid().ToString();
            options.ServiceName = configuration["ConsulRegistry:ServiceName"];
            options.ServiceAddress = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") != null ? 
                Environment.GetEnvironmentVariable("ASPNETCORE_URLS") : configuration["ConsulRegistry:ServiceAddress"];
            options.HealthCheckAddress = configuration["ConsulRegistry:HealthCheckAddress"];
            options.RegistryAddress = configuration["ConsulRegistry:RegistryAddress"];//"http://localhost:8500";
            options.ServiceTags = ["后台作业服务", "定时作业后台执行"];//标签
        });
        //心跳检测
        context.Services.AddHealthChecks();
    }

    /// <summary>
    /// 注册hangfire
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
    {
        //创建表，程序第一次启动自动生成相关hangfire表
        var storage = new SqlServerStorage(
          configuration.GetConnectionString("HangFireJob"),
           new SqlServerStorageOptions
           {
               // TransactionIsolationLevel = IsolationLevel.ReadCommitted, // 事务隔离级别。默认是读取已提交。
               QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
               JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
               CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
               PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
               DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
               TransactionTimeout = TimeSpan.FromMinutes(20),             //- 交易超时。默认为1分钟。
               SchemaName = "hx"
           }
         );
        //详细配置
        context.Services.AddHangfire(x =>
        {
            //x.UseRedisStorage(connectionString, new RedisStorageOptions()
            //{
            //    //活动服务器超时时间,要引入Hangfire.Redis 包
            //    InvisibilityTimeout = TimeSpan.FromMinutes(60),
            //    Db = int.Parse(configuration["Hangfire:Redis:Db"])
            //});
            x.UseStorage(storage)
            //.WithJobExpirationTimeout(TimeSpan.FromHours(6))
            //.UseHangfireHttpJob() //第三方插件
                .UseSerilogLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()// 使用简单的程序集名称类型序列化器
                .UseRecommendedSerializerSettings();// 使用推荐的序列化器设置
                                                    //.UseRedisStorage("127.0.0.1:6379")
        });

        #region 其他配置
        ////context.Services.AddHangfire(config =>
        ////{
        ////    config.UseSqlServerStorage(configuration.GetConnectionString("HangFireJob"));//简单配置
        ////});
        //BackgroundJobServer jobServer = new BackgroundJobServer(new BackgroundJobServerOptions()
        //{
        //    Queues = new string[] { "queueone", "default" },
        //    ServerName = "Test",
        //    WorkerCount = Environment.ProcessorCount * 5
        //}, storage);
        //设置队列
        //context.Services.AddHangfireServer(options =>
        //{
        //    options.Queues = new[] { "queueone", "queuetwo", "default" };
        //    options.WorkerCount = Environment.ProcessorCount * 5; //并发任务数  --超出并发数。将等待之前任务的完成  (推荐并发线程是cpu 的 5倍)
        //    options.ServerName = "default";
        //});
        #endregion
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        //if (MultiTenancyConsts.IsEnabled)
        //{
        //    app.UseMultiTenancy();
        //}
        app.UseAbpRequestLocalization();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthScopes("HangFireJob");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        //心跳检测
        app.UseHealthChecks("/HealthCheck");

        app.UseHangfireServer();

        //打开仪表盘-简单方式
        //app.UseHangfireDashboard();

        //拦截器验证 通过验证直接进入仪表盘，没通过验证返回 401
        //app.UseHangfireDashboard("/hangfire", new DashboardOptions()
        //{
        //    Authorization = new[] { new CustomHangfireAuthorizeFilter() }
        //    ,
        //    DashboardTitle = "合心任务调度中心"
        //});

        //定时任务RecurringJob调用方式1
        var service = context.ServiceProvider;
        // service.UseHangfireTest();

        // service.UseHangfireJob();

        // RecurringJob.AddOrUpdate("jobtwo", () => Console.WriteLine($"APS.NET Core Hangfire"), Cron.Minutely(), TimeZoneInfo.Local);

        ////添加面板的打开权限。不是所有人都可以打开面板。可以操作后台任务。
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            //可视化界面 输入正确进入仪表盘
            Authorization = new[]
           {
                 new Hangfire.Dashboard.BasicAuthorization.BasicAuthAuthorizationFilter(new Hangfire.Dashboard.BasicAuthorization.BasicAuthAuthorizationFilterOptions
                 {
                    SslRedirect = false,          // 是否将所有非SSL请求重定向到SSL URL
                    RequireSsl = false,           // 需要SSL连接才能访问HangFire Dahsboard。强烈建议在使用基本身份验证时使用SSL
                    LoginCaseSensitive = false,   //登录检查是否区分大小写
                    Users = new[]
                    {
                        new Hangfire.Dashboard.BasicAuthorization.BasicAuthAuthorizationUser
                        {
                            Login = AppSettings.Hangfire.Login,
                            PasswordClear =  AppSettings.Hangfire.Password
                        }
                    }
                 })
               },
            DashboardTitle = "任务调度中心",
            //AppPath= AppSettings.Hangfire.AppPath,//更改回站点连接
            IsReadOnlyFunc = (context) => false  // 设置仪表盘为可写模式

        });






    }
}
