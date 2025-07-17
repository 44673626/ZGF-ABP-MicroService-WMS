using Autofac.Core;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using WMS.BaseService.CommonManagement.AbpExceptionFilters;
using WMS.BaseService.CommonManagement.UploadBlobFiles;
using WMS.BaseService.Consts;
using WMS.BaseService.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Polly;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Dapper;

namespace WMS.BaseService;

[DependsOn(
    typeof(WMSBaseApplicationModule),
    typeof(WMSBaseEntityFrameworkCoreModule),
    typeof(WMSBaseHttpApiModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),//集成redis
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpBlobStoringModule),//注册BLOB服务（瞬态服务）
    typeof(AbpBlobStoringFileSystemModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpDapperModule),//使用Dapper
    typeof(AbpSwashbuckleModule)
    )]
public class ABPVNextHttpApiHostModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();//应用程序的托管环境
        var configuration = context.Services.GetConfiguration();

        Configure<SwaggerUIOptions>(c =>
        {
            configuration.Bind("SwaggerUI", c);
            ////SupportedSubmitMethods属性定义了Swagger UI中允许提交的HTTP方法类型。将其设置为空数组将隐藏所有HTTP方法的提交按钮。
            //c.ConfigObject.SupportedSubmitMethods = new SubmitMethod[] { };
            ////AdditionalItems是一个字典，允许你添加任何额外的键值对。在这个例子中，添加了一个键为"swaggerUIFoo"，值为"bar"的项。这些自定义项可以用来扩展Swagger UI的功能或提供一些自定义的配置
            //c.ConfigObject.AdditionalItems.Add("swaggerUIFoo", "bar");
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
        if (bool.Parse(configuration["AppSettings:MiniProfiler:Enabled"]))
        {
            //接口性能监测
            context.Services.AddMiniProfiler(options =>
             options.RouteBasePath = "/profiler"
            );
        }

        context.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;//解决后端传到前端全大写,设置值 null
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);//解决后端返回数据中文被编码
        });


        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<WMSBaseDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}WMS.BaseService.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<WMSBaseDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}WMS.BaseService.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<WMSBaseApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}WMS.BaseService.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<WMSBaseApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}WMS.BaseService.Application", Path.DirectorySeparatorChar)));
            });
        }

        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"],
            new Dictionary<string, string>
            {
                {"ABPVNext", "ABPVNext API"}
            },
            options =>
            {
                ConfigureSwaggerServices(options);
            });
        //设置语言
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

        });
        //接口JWT认证
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "BusinessService";
            });
        //post方式防伪攻击
        context.Services.Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });
        //为应用程序设置缓存键前缀，每个应用程序有自己的缓存
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "ABPVNext:";
        });
        //数据保护构建器
        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("ABPVNext");
        if (!hostingEnvironment.IsDevelopment())
        {
            //判断是否在非开发环境中启用Redis缓存
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "ABPVNext-Protection-Keys");//数据保护服务的密钥
        }

        #region 新注册组件-2024
        //应用服务自动转换为控制器
        ConfigureConventionalControllers();
        //注册日志2024
        ConfigSerilog(configuration);

        //是否开启多租户,默认true
        Configure<AbpMultiTenancyOptions>(options =>
        {
            //options.IsEnabled = MultiTenancyConsts.IsEnabled;
            options.IsEnabled = bool.Parse(configuration["AppSettings:MultiTenancy:Enabled"]);
        });
        //注册CAP
        ConfigureCap(context);
        //自定义拦截,重写过滤器2024.2.1
        if (bool.Parse(configuration["AppSettings:Log:EapExceptionEnabled"]) == true)
        {
            //在AppModule替换默认的AbpExceptionFilter为新的EapExceptionFilter过滤器
            context.Services.AddMvc(options =>
            {
                options.Filters.ReplaceOne(x => (x as ServiceFilterAttribute)?.ServiceType?.Name == nameof(AbpExceptionFilter),
                    new ServiceFilterAttribute(typeof(EapExceptionFilter)));
            });
        }
        //导入操作时上传文件的注册容器
        ConfigureBLOBServices();
        //context.Services.AddTransient<IFileStorageBlobAppService, FileStorageBlobAppService>();
        //存储模板的容器（独立于上传文件容器）
        ConfigureBLOBTemplateServices();

        #endregion

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }


    #region 私有注册方法2024
    /// <summary>
    /// 应用服务自动转换为控制器（实现路由写在Application服务层）
    /// </summary>
    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options
                .ConventionalControllers
                .Create(typeof(WMSBaseApplicationModule).Assembly, opts
                     =>
                { opts.RootPath = "abpvnext"; });
        });

    }
    /// <summary>
    /// 注册分布式事务CAP(替换abp自带的事件总线)
    /// </summary>
    /// <param name="context"></param>
    private void ConfigureCap(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        //用于控制CAP是否启用
        if (string.IsNullOrWhiteSpace(configuration["AppSettings:Cap:UseCap"]) || bool.Parse(configuration["AppSettings:Cap:UseCap"]) == false) return;

        context.Services.AddCap(x =>
        {
            //连接CAP数据库
            x.UseSqlServer(configuration.GetConnectionString("CAPConnect"));
            //连接RabbitMQ：地址、端口、用户名和密码等配置
            x.UseRabbitMQ(mq =>
            {
                mq.HostName = configuration["RabbitMQ:Connections:Default:HostName"];
                mq.Port = int.Parse(configuration["RabbitMQ:Connections:Default:Port"]);
                mq.UserName = configuration["RabbitMQ:Connections:Default:UserName"];
                mq.Password = configuration["RabbitMQ:Connections:Default:Password"];
            });
            //默认分组名，将自动生成RabbitMq中队列，此值不配置时，默认值为当前程序集的名称
            x.GroupNamePrefix = configuration["RabbitMQ:GroupName"]; ;
            //默认值60秒，消费失败，设置时间间隔进行重试消费（默认情况，重试是发送和消费失败4分钟重试，为了避免延迟）
            x.FailedRetryInterval = 60;
            x.Version = "abpv1";//通过本地数据表的Version字段进行版本隔离
            //CAP可视化面板（重要：面板上有"重新消费"功能，重试次数用完后，可重新消费）
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            bool auth = !hostingEnvironment.IsDevelopment();
            x.UseDashboard(options =>
            {
                //options.UseAuth = auth;
                options.PathMatch = "/abpcap";//默认地址:http://localhost:端口/cap
            });
            if (configuration["AppSettings:Cap:UseCapCallBack"] == "true")
            {
                x.FailedThresholdCallback = (failedinfo) =>
                {
                    var _capPublisher = failedinfo.ServiceProvider.GetService<ICapPublisher>();
                    var header = new Dictionary<string, string>()
                    {
                        ["header.error.msgid"] = failedinfo.Message.Headers["cap-msg-id"],
                        ["header.error.msgname"] = failedinfo.Message.Headers["cap-msg-name"]
                    };
                    //发布消息失败记录日志或加入邮件发送功能（设置失败类型Publish/Subscribe）
                    if (failedinfo.MessageType == MessageType.Publish)
                    {
                        _capPublisher.Publish("publish-dead-letter-queue", failedinfo.Message.Value, header);//发布的死信队列
                    }
                    if (failedinfo.MessageType == MessageType.Subscribe)
                    {
                        _capPublisher.Publish("subscribe-dead-letter-queue", failedinfo.Message.Value, header);//订阅的死信队列
                    }
                };
            }

            //设置成功信息的删除时间默认24*3600（秒），为保证系统性能，数据会定期清理。
            x.SucceedMessageExpiredAfter = 60 * 60;

            //设置失败重试次数
            x.FailedRetryCount = 50;
            //针对重试机制处理4分内处理不完的消息，可以通过FallbackWindowLookbackSeconds加大阀值，默认60*4秒
            //x.FallbackWindowLookbackSeconds = 60 * 6;

        });

    }
    /// <summary>
    /// 从Program中稳步到这里，读取配置节，Serilog记录日志提供三种方式，任选其一即可
    /// </summary>
    /// <param name="_configuration"></param>
    private void ConfigSerilog(IConfiguration _configuration)
    {
        if (bool.Parse(_configuration["AppSettings:Log:WriteToSqlEnabled"]) == true)
        {
            Log.Logger = new LoggerConfiguration()
            #if DEBUG
                                   .MinimumLevel.Debug()
            #else
                                            .MinimumLevel.Information()
            #endif
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
           .Enrich.FromLogContext()
            //方式：日志写入数据库
            .WriteTo.MSSqlServer(
                      connectionString: _configuration.GetConnectionString("Default"),
                      sinkOptions: new MSSqlServerSinkOptions
                      {
                          AutoCreateSqlTable = true,
                          TableName = "AbpExceptions",
                          LevelSwitch = new Serilog.Core.LoggingLevelSwitch(LogEventLevel.Error),
                      }
                  )
           .WriteTo.Async(c => c.Console())
           .CreateLogger();
        }
        else if (bool.Parse(_configuration["AppSettings:Log:WriteToSeqEnabled"]) == true)
        {
            Log.Logger = new LoggerConfiguration()
            #if DEBUG
                                   .MinimumLevel.Debug()
            #else
                                            .MinimumLevel.Information()
            #endif
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
           .Enrich.FromLogContext()
            //方式：使用Seq,如果使用可提供具体操作文档，借助第三方工具，可设置日志过期时间，自动清理日志文件等，有可视化界面
            .WriteTo.Seq(serverUrl: _configuration["SerilogAbp:ServerUrl"],
                   apiKey: _configuration["SerilogAbp:ApiKey"],
                   controlLevelSwitch: new Serilog.Core.LoggingLevelSwitch(_configuration.GetValue<LogEventLevel>("SerilogAbp:MinimumLevel")))
           .WriteTo.Async(c => c.Console())
           .CreateLogger();
        }
    }
    /// <summary>
    /// swagger
    /// </summary>
    /// <param name="options">属性设置</param>
    private static void ConfigureSwaggerServices(SwaggerGenOptions options)
    {
        //--加入swagger文档接口注释（设置：右键项目>属性>生成>输出>XML文档文件路径，XML将自动生成）
        var basePath = PlatformServices.Default.Application.ApplicationBasePath;
        var xmlPath = Path.Combine(basePath, "WMS.BaseService.HttpApi.Host.xml");//host层说明文档
        options.IncludeXmlComments(xmlPath, true);
        var xmlPathContracts = Path.Combine(basePath, "WMS.BaseService.Application.Contracts.xml");//接口层文档说明
        options.IncludeXmlComments(xmlPathContracts, true);
        var xmlPathController = Path.Combine(basePath, "WMS.BaseService.HttpApi.xml");//api层文档说明
        options.IncludeXmlComments(xmlPathController, true);
        var xmlPathApplication = Path.Combine(basePath, "WMS.BaseService.Application.xml");//服务层文档说明
        options.IncludeXmlComments(xmlPathApplication, true);
        //--swagger文档完成注释--
        options.SwaggerDoc("Infra", new OpenApiInfo { Title = "基础服务接口", Version = "v1" });//swagger分组，第一个参数，要和appsetting.json文件中配置保持一致，否则报错
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "ABPVNext API", Version = "v1" });//swagger分组
        options.DocInclusionPredicate((docName, description) => true);
        options.CustomSchemaIds(type => type.FullName);

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "请输入JWT令牌，例如：Bearer 12345abcdef",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
        options.DocInclusionPredicate((docName, description) =>
        {
            if (!description.TryGetMethodInfo(out MethodInfo method))
            {
                return false;
            }

            /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
               * DeclaringType只能获取controller上的特性
               * 我们这里是想以action的特性为主
               * */
            var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
            if (version.Any())
            {
                return version.Any(v => v == docName);
            }
            else if (docName.Equals("v1"))
            {
                return true;
            }
            //这里获取action的特性
            var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
            if (actionVersion.Any())
            {
                return actionVersion.Any(v => v == docName);
            }
            return false;
        });
    }

    /// <summary>
    /// 用于存储模板文件（独立于上传文件容器）
    /// </summary>
    /// <param name="configPath"></param>
    private void ConfigureBLOBTemplateServices()
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.Configure<BlobTemplateFileContainer>(configuration =>
            {
                configuration.UseFileSystem(fileSystem =>
                {
                    var filestreampath = Environment.CurrentDirectory + @"\wwwroot\template";
                    if (!Directory.Exists(filestreampath))
                    {
                        Directory.CreateDirectory(filestreampath);
                    }
                    fileSystem.BasePath = filestreampath;
                });
            });

        });
    }

    /// <summary>
    /// 导入操作时上传文件的容器
    /// </summary>
    /// <param name="configPath">路径</param>
    private void ConfigureBLOBServices()
    {
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.Configure<BlobCommonFileContainer>(configuration =>
            {
                configuration.UseFileSystem(fileSystem =>
                {
                    var filestreampath = Environment.CurrentDirectory + @"\wwwroot\files";
                    if (!Directory.Exists(filestreampath))
                    {
                        Directory.CreateDirectory(filestreampath);
                    }
                    fileSystem.BasePath = filestreampath;
                });
            });

        });
    }

    #endregion

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
        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseAbpRequestLocalization();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.IndexStream = () =>
            GetType().Assembly.GetManifestResourceStream("WMS.BaseService.index.html");
        });
        //app.UseAbpSwaggerUI(options =>
        //{
        //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
        //    options.DocExpansion(docExpansion: DocExpansion.List);//修改界面时自动折叠
        //    var configuration = context.GetConfiguration();
        //    options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
        //    options.OAuthScopes("ABPVNext");
        //});
        app.UseAuditing();
        app.UseMiniProfiler();//启用接口性能分析中间件
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
