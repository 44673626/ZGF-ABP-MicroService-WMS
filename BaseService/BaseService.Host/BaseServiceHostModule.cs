using BaseService.EntityFrameworkCore;
using BaseService.Systems.MessageManagement;
using Comm.ConsulConfig;
using Comm.ConsulConfig.Registry.Options;
using DotNetCore.CAP.Messages;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;
//using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BaseService
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BaseServiceApplicationModule),
        typeof(BaseServiceEntityFrameworkCoreModule),
        typeof(BaseServiceHttpApiModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(CommConsulModule), //引入consul
        typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpAspNetCoreSerilogModule)
    )]
    public class BaseServiceHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            IdentityModelEventSource.ShowPII = true;

            //配置swagger分组、DTO展示方式等配置
            //Configure<SwaggerUIOptions>(c =>
            //{
            //    configuration.Bind("SwaggerUI", c);
            //});

            #region SignalR配置
            Configure<AbpSignalROptions>(options =>
            {
                options.Hubs.AddOrUpdate(
                    typeof(MessageHub), //Hub type
                    config => //Additional configuration
                    {
                        config.RoutePattern = "/myhub"; //override the default route
                        config.ConfigureActions.Add(hubOptions =>
                        {
                            //Additional options
                            hubOptions.LongPolling.PollTimeout = 
                                TimeSpan.FromSeconds(30);
                        });
                    }
                );
            });
            #endregion

            //去防伪
            context.Services.Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidate = false;
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = true;
            });

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = "BaseService";
                });

            context.Services.AddSwaggerGen(options =>
            {
                //是否启用swagger
                if (string.IsNullOrWhiteSpace(configuration["AppSettings:UseSwagger"]) || bool.Parse(configuration["AppSettings:UseSwagger"]) == false) return;

                //options.SwaggerDoc("base", new OpenApiInfo { Title = "基础服务", Version = "v1" });//swagger分组，其中base保持和配置文件一致 
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BaseService Service API", Version = "v1" });
                
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
                //排除XX.HttpApi程序集中的API(如果引用其他程序集httpapi的Nuget包)
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    // 获取包含类的程序集名称
                    var assemblyName = controllerAction?.MethodInfo.DeclaringType?.Assembly.GetName().Name;

                    // 排除特定的程序集
                    return !(assemblyName == "XX1_Business.HttpApi" || assemblyName == "XX2.HttpApi");
                });
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            //context.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration["Redis:Configuration"];
            //});

            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabledForGetRequests = true;
                options.ApplicationName = "BaseService";
            });
            //调用Hims3的认证接口，取token信息用
            context.Services.AddHttpClient("AuthClientHttpUrl", c =>
            {
                c.BaseAddress = new Uri(configuration["GetIndentityToken:Default:AuthClientHttpUrl"]);
            });

            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
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

            //var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            //context.Services.AddDataProtection()
            //    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });

            Configure<PermissionManagementOptions>(options =>
            {
                options.IsDynamicPermissionStoreEnabled = true;
            });

            //重置密码功能 
            ConfigurePasswordSet(context);
            ConfigureCap(context);//注册CAP
            ConfigureConsul(context, configuration);//集成consul
            context.Services.AddHttpClient("IMMPHttpClent", c =>
            {
                c.BaseAddress = new Uri(configuration["RabbitMQ:HttpClientUrl"]);
            });
        }

        #region 中间件注册
        /// <summary>
        /// 设置密码强度
        /// </summary>
        /// <param name="context"></param>
        private void ConfigurePasswordSet(ServiceConfigurationContext context)
        {
            context.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });
        }
        private void ConfigureConsul(ServiceConfigurationContext context, IConfiguration configuration)
        {
            // consul注册
            Configure<ServiceRegistryOptions>(options =>
            {
                options.ServiceId = Guid.NewGuid().ToString();
                options.ServiceName = configuration["ConsulRegistry:ServiceName"];
                options.ServiceAddress = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") != null ? Environment.GetEnvironmentVariable("ASPNETCORE_URLS") : configuration["ConsulRegistry:ServiceAddress"];
                options.HealthCheckAddress = configuration["ConsulRegistry:HealthCheckAddress"];
                options.RegistryAddress = configuration["ConsulRegistry:RegistryAddress"];
                options.ServiceTags = new string[] { "基础服务项目", "用户管理、角色、权限、租户和菜单等" };//标签
            });
            //心跳检测
            context.Services.AddHealthChecks();
        }
        /// <summary>
        /// 使用分布式发布订阅CAP
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureCap(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            //开关
            if (string.IsNullOrWhiteSpace(configuration["AppSettings:Cap:UseCap"]) || bool.Parse(configuration["AppSettings:Cap:UseCap"]) == false) return;

            context.Services.AddCap(x =>
            {
                //连接CAP数据库
                x.UseSqlServer(configuration["ConnectionStrings:CAPConnect"]);
                //连接RabbitMQ
                x.UseRabbitMQ(mq =>
                {
                    mq.HostName = configuration["RabbitMQ:Connections:Default:HostName"];
                    mq.Port = int.Parse(configuration["RabbitMQ:Connections:Default:Port"]);
                    mq.UserName = configuration["RabbitMQ:Connections:Default:UserName"];
                    mq.Password = configuration["RabbitMQ:Connections:Default:Password"];
                });
                //默认分组名，将自动生成RabbitMq中队列，此值不配置时，默认值为当前程序集的名称
                x.GroupNamePrefix = configuration["RabbitMQ:GroupName"];
                //默认值60秒，消费失败，设置时间间隔进行重试消费（默认情况，重试是发送和消费失败4分钟重试，为了避免延迟）
                x.FailedRetryInterval = 60;
                //x.Version = "v1";
                //CAP面板
                x.UseDashboard();
                //设置处理成功的数据在数据库中保存的时间（秒），为保证系统性能，数据会定期清理。
                x.SucceedMessageExpiredAfter = 24 * 3600;
                //设置失败重试次数
                x.FailedRetryCount = 50;
                x.UseDashboard(options =>
                {
                    options.PathMatch = "/cap";//默认地址:http://localhost:端口/cap,可自定义命名
                });
                if (bool.Parse(configuration["AppSettings:Cap:UseCapCallBack"]) == true)
                {
                    //CallBack机制，如果重试完还是失败会进入这里，以下功能用于模拟死信队列（CAP目前研究没有设置死信队列的地方）
                    //处理进死信队列里面，后期可以手动处理
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
            });

        }
        #endregion

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseMultiTenancy();

            //心跳检测
            app.UseHealthChecks("/HealthCheck");

            //app.UseConfiguredEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<MessageHub>("/api/v1/sys/signalr-hubs", options =>
            //    {
            //        options.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
            //    });
            //});

            app.Use(async (ctx, next) =>
            {
                var currentPrincipalAccessor = ctx.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
                var map = new Dictionary<string, string>()
                {
                    { "sub", AbpClaimTypes.UserId },
                    { "role", AbpClaimTypes.Role },
                    { "email", AbpClaimTypes.Email },
                    { "name", AbpClaimTypes.UserName },
                };
                var mapClaims = currentPrincipalAccessor.Principal.Claims.Where(p => map.Keys.Contains(p.Type)).ToList();
                currentPrincipalAccessor.Principal.AddIdentity(new ClaimsIdentity(mapClaims.Select(p => new Claim(map[p.Type], p.Value, p.ValueType, p.Issuer))));
                await next();
            });

            app.UseAbpRequestLocalization();
            app.UseSwagger();
            //app.UseAbpSwaggerUI();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BaseService Service API");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();

            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>()
                        .SeedAsync();
                }
            });
        }
    }
}
