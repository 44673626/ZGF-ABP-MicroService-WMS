

## 前言：.NET最流行的微服务架构
本系统开发使用ABPVNext微服务框架，ABP版本8.3.4最新版本（基于.net8，不考虑最新版本ABP9）,功能开发中，并会长期技术支持服务。
主要特性：最新.net core框架，稳定强大；基于abp vNext、vue-element-admin，成熟的开发社区；微服务架构设计，DDD实践；统一授权、身份、权限管理，简化微服务开发；并支持多租户。

---
# 基于ABPVnext微服务开的WMS系统
-------------

> 请关注我们的团队，给一个星星的点赞，您的关注点赞是我们开发的动力！
> 
  博客：[梦想代码][(https://www.cnblogs.com/netcore-vue)](https://www.cnblogs.com/netcore-vue)  
  
  持续关注和分享：团队致力于开发一套完整的WMS系统，使用微服务架构(前后端分离开发)，欢迎有识之士加入我们的团队！


### 项目启动初始条件（必须）  

- 启动Consul服务
  启动consul服务发现，安装consul后，执行consul.exe后(找不到可以加QQ支持群里要)，在该目录下执行cmd，输入启动命令：consul agent -dev，运行地址：http://localhost:8500/
  说明：因为所有服务都走服务发现，这个是必须先启动，才能运行整个微服务。
- 启动Redis
  执行redis安装文件，本地启动即可

### 前端运行效果
前端使用Vue2.0，使用经典易上手的vue-admin-element框架，前后端分离开发，支持多租户等功能。
> 首页,公司输入框内可输入租户信息
![image](https://github.com/user-attachments/assets/7f1dcc45-0c68-4cd8-9e9b-aa7f6613934a)
> 用户名:admin 密码：1q2w3E*
![image](https://github.com/user-attachments/assets/a7b97e61-4236-4aaf-a50c-8bd2cd004e19)

#### ABPVNext微服务架构:  
ABP vNext 框架是一个基于ASP.NET核心的完整基础设施，通过遵循软件开发最佳实践和最新技术来创建现代web应用程序和API，它完全基于 ASP.NET Core，设计更加合理，更加细粒度的模块化设计。
ABP框架提供了基础设施使基于领域驱动设计的开发更易实现。
> ABP框架遵循DDD原则和模式去实现分层应用程序模型,该模型由四个基本层组成:
- 表示层: 为用户提供接口. 使用应用层实现与用户交互.
- 应用层: 表示层与领域层的中介,编排业务对象执行特定的应用程序任务. 使用应用程序逻辑实现用例.
- 领域层: 包含业务对象以及业务规则. 是应用程序的核心.
- 基础设施层: 提供通用的技术功能,支持更高的层,主要使用第三方类库.

简单总结：ABPVNext框架使用DDD的思想，将业务逻辑和数据持久化分离，使系统更易于维护和扩展。

  --- ![image](https://github.com/user-attachments/assets/0473364f-76e5-4648-a2ef-2ab3b489ea85)---

### 系统使用的技术栈（最新）
> 分布式日志Serilog
- 写入文本（防止过大打不开，按日期及大小进行分割）--默认方式
- Serilog方式写入数据库（设置日志级别过滤）
- 第三方分布式日志可视化工具Seq（很给力），编写了具体操作和使用文档，对于日志可定期清除，日志查询丰富，定位准确。
- 自定义数据库日志（重写过滤器）可根据实际需求进行自定义更改
- 以上日志类型可通过配置文件中设置开关来控制
  ![image](https://github.com/user-attachments/assets/0d4d07f4-03cf-447c-867e-5e4d7a72a1c9)
  ![image](https://github.com/user-attachments/assets/d5f00224-aa1e-4788-b962-9033fad4fd78)
  ![image](https://github.com/user-attachments/assets/5db8d784-b68b-44bd-8c71-f81c5ef1aee3)

> CAP+RabbitMq分布式事务（使用CAP8.0版本，支持.net6、.net8）
- 替代ABP自带的分布式事务总线EventBus，主要原因，实现事务的补偿及最终一致性
- 启动配置：设置消息重试次数、数据保存时间、消费间隔等
- 实现发布-订阅模式（实现通信解耦）
1，发布CapPublisher 接口
2，消费者-使用 CapSubscribeAttribute 来订阅
- 实现订阅分组Group功能（用于定义队列，Mq中自动生成该队列）
- 实现CAP面板的注册和使用（重要设置：面板上可以对过期消息进行重新消费功能，类似死信功能）
说明：
1，cap7.0以后版本Dashboard 支持对延迟消息查看和操作
2，Dashboard 添加新图表支持 Metric 实时查看
- FailedCallback回调函数： 如何处理失败的消息,实现主动干预，例如发邮件等功能
- CAP结合ABP的UOW事务使用，保证业务和数据库事务一致性
- RabbitMq使用（CAP如何结合使用及相关设置）-重要：
交换机（主推Topic主题）、Binding Key绑定键、Queue队列、Routing Key路由键等使用
延时加载待考虑（cap7.0支持延时加载）
- 支持发布延迟消息（CAP7.0+版本支持）-防止阻塞
- CAP8.0支持自定义回溯时间窗-针对重试机制处理4分内处理不完的消息，可以通过FallbackWindowLookbackSeconds加大阀值
- 支持手动 Start/Stop CAP 进程（cap7.0+）
（通过这个特性，可以延申出来的应用场景有很多，比如在系统在某个时候开始发送/消费消息等）
- 问题：无限重试问题（如果订阅者移除后消息无限重试）
CAP8.0已经解决此BUG,同样支持.net6/.net8
![image](https://github.com/user-attachments/assets/f5b6536f-43d7-465b-927c-e1e8c35f1967)
![image](https://github.com/user-attachments/assets/a0279dec-6822-44c0-a1a6-1d8fe3dbc4c2)

> 分布式缓存Redis
- 分布式锁：ABP当前的分布式锁实现基于DistributedLock库.
- DistributedLock.SqlServe锁：使用 Microsoft SQL Server-2.0
- 封装ABP自带的缓存IDistributedCache机制Volo.Abp.Caching，封装到CURD中
1，基础：添加、修改、删除缓存数据
2，实现 GetOrAddAsync() 方法从缓存中取数据，缓存中不存从原始数据中取数据
3，设置缓存时间、应用程序缓存前缀（进行隔离）
4，无论选择哪种缓存的实现，应用都将使用 IDistributedCache 接口与缓存进行交互。
- 应用-分布式Redis缓存：
实现IDistributedCache 接口与Redis的集成，调用AddStackExchangeRedisCache注册Redis缓存，并替换其默认实现为AbpRedisCache
- Redis配置，可设置开关是否启用，默认true
- 监控界面:一种使用：Another-Redis-Desktop-Manager.1.6.3（github上第三方）；另一种：RedisDesktopManager（官方）
  ![image](https://github.com/user-attachments/assets/835f5e5b-f9f1-4687-a4d2-46235023d344)

> Apollo分布式配置
- 中间件形式引入
- Apollo配置文件中Enable来控制是否启用Apollo，不启用将默认读取原appsetting.json
  ![image](https://github.com/user-attachments/assets/0e5f70ec-e83d-4f63-8abf-54c541e8f94a)

> 后台监控任务HangFire
- 数据库持久化（自动创建表）：
  ``` xml
  services.AddHangfire(x => x.UseSqlServerStorage(connection));
  ```
- 替代ABP自带的后台作业BackgroundJob
- 同步执行--单次任务（BackgroundJob属于一次性“消费”的任务）
- 定期执行RecurringJob--分钟级别循环执行（类似数据库定期任务），相比数据库的定期任务，易于迁移
- 监控面板：app.UseHangfireDashboard();
  ![image](https://github.com/user-attachments/assets/77b5ab8b-d4b1-42a2-b78f-26204f7d4fbf)

> EFCore大数据处理组件：
EFCore.BulkExtensions
- EntityFrameworkCore扩展：批量操作(插入，更新，删除，读取，更新，同步)和批处理(删除，更新)
- A.增加：BulkInsert
　B.修改：BulkUpdate，需要传入完整实体,不传的字段就会被更新为空
　C.增加或修改：BulkInsertOrUpdate (主键存在执行update，不存在执行insert)
　D.删除：BulkDelete 和 Truncate(删除整张表)
- 处理大数据优于EFCore
> 文件导入及导出
- 第三方开源免费组件：Magicodes.IE
参见：https://gitee.com/magicodes/Magicodes.IE(好东西，一定要分享)
- Magicodes.IE支持Dto导入导出以及动态导出,支持Excel、Word、Pdf、Csv和Html
- BLOB二进制形式的文件存储-ABP类型化容器应用
- 导入文件的模板上传和下载（独立于普通上传文件）
- Excel文件下载、导入及导出操作（DTO方式）
> 多租户管理
- 是否启动租户--配置文件中设置开关
- 租户的管理（租户创建、修改和查询、租户数据库查询等）
- 租户权限-加组件后可迁移生成
- 租户的独立数据库管理--提供接口
- 租户切换--获取当前租户，如何调用Change方法切换
- 多租户表结构（实现一键迁移）
- 租户表结构变化（上传SQL文本，实现租户数据表字段的新增或修改）
  ![image](https://github.com/user-attachments/assets/862b9048-2601-433c-893a-15b7e62872eb)

> 数据库集成及自动迁移操作
- Add-Migration，用于生成新的数据表或修改现有表结构的迁移文件（要支持多数据上下文的迁移）
- Update-Database -context {DbContext} -更新表结构，要支持多数据上下文的迁移
- SQL脚本迁移：Script-Migration -From {迁移文件} -To {迁移文件}（可实现数据表结构的增量更新）
- 集成Dapper（用于执行Sql语句和存储过程，进行封装，可重用方便使用）和ABP的UOW使用时要实现同步
> ABP本地化，多语言
- Localization文件夹：配置语言json，例如en.json/zh-hans.json；设置资源类库，比如BookStoreResource。
- 配置Module：使用
注入例如：readonly IStringLocalizer<OpenIddictResponse> L
![image](https://github.com/user-attachments/assets/3417af77-98c0-4c66-8d34-41d9e979cf4e)

> 外部网关/内部网关
- 外部网关-对外接口:包括请求头设置，网关鉴权设置，Ocelot配置等等，提供前端各服务的接口；
  ![image](https://github.com/user-attachments/assets/9e625b91-26bb-44eb-b1c1-cae2abd0a03d)

- 内部网关-服务内通信，包括带token权限的服务间通信；
  ![image](https://github.com/user-attachments/assets/104d20bb-6c5b-4720-96c6-2028d76ce5f0)
![image](https://github.com/user-attachments/assets/802915db-0493-437d-9734-f44429446163)

> Ocelot集成Consul(服务发现)
- 服务发现（网关中配置）：网关项目引入包：Ocelot.Provider.Consul；
- 网关中appsettings.json中配置：
``` xml
"GlobalConfiguration": {
    "ServiceDiscoveryProvider": {
      "Scheme": "http",
      "Host": "localhost", //你的Consul的ip地址
      "Port": 8500, //你的Consul的端口
      "Type": "Consul" //类型
    }
```
- 网关中配置请求的微服务（替换原下游地址）：
``` xml
  {
      "DownstreamPathTemplate": "/connect/token",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/api/connect/token",
      "UpstreamHttpMethod": [ "Post" ],
      "ServiceName": "authService", //请求服务名称
      "UseServiceDiscovery": true, //可以不用设置，默认开启 服务发现 功能
      "LoadBalancerOptions": {
        "Type": "RoundRobin" //负载均衡算法
      }
    },
```
- 网关中HostModule配置，添加AddConsul()具体：
  ``` xml
   context.Services.AddOcelot(context.Services.GetConfiguration()).AddConsul()
  ```
> 服务注册（微服务中配置）
- 引入封装完的nuget包：Com.ConsulConfig
该包封装了consul服务注册，核心类：AgentServiceRegistration（服务名、地址、端口、标签和心跳监测等）
- HostModule中的DependsOn加入：typeof(HxConsulModule)
- 注册心跳检验：
1，context.Services.AddHealthChecks();
2，app.UseHealthChecks("/HealthCheck");
- 服务心跳，监控界面:启动consul命令：consul agent -dev 
（其它命令参考https://www.consul.io/commands）,本地运行：http://localhost:8500/，效果图
![aa40139435a2523bf85160333e4582c](https://github.com/user-attachments/assets/aa6b72b3-d096-433b-bdfc-becfb27d4511)


###前端，将使用vue-admin-element框架，vue2.0,后期会支持vue3.0！

### ABP环境安装
> 安装脚手架
命令：
 ``` xml
dotnet tool install -g Volo.Abp.Cli
 ```
> 生成模板
- 在线生成（abp官网：abp.io）
  项目生成可以使用官网在线生成，或用命令生成，前提是已经安装了CLI脚手架。
	官网上可以看到，生成项目时有多种选，比如数据库的选择等
![image](https://github.com/user-attachments/assets/ed81c370-0a4b-47fd-9912-927e24dd749b)
> 使用脚手架命令
使用命令生成模板，命令如下：
 ``` xml 
abp new XX.BaseService -t module --no-ui --version 8.0.0
 ```
可以指定版本生成，如上述代码中的version中设置，数据库默认是SQL Server，--no-ui选项指定不包含UI层。
> 生成项目的依赖关系
![image](https://github.com/user-attachments/assets/ab208ea5-5005-4c8c-a475-f6151e255e93)
 根据上图的依赖关系，初学者必备知识以下知识：
 - Domain.Shared 项目
项目包含常量,枚举和其他对象,这些对象实际上是领域层的一部分,但是解决方案中所有的层/项目中都会使用到。
 - .Domain 项目
解决方案的领域层. 它主要包含 实体, 集合根, 领域服务, 值类型, 仓储接口 和解决方案的其他领域对象.
例如 Book 实体和 IBookRepository 接口都适合放在这个项目中.
说明：它依赖 .Domain.Shared 项目,因为项目中会用到它的一些常量,枚举和定义其他对象.
- Application.Contracts 项目
项目主要包含 应用服务 interfaces 和应用层的 数据传输对象 (DTO). 它用于分离应用层的接口和实现. 这种方式可以将接口项目做为约定包共享给客户端.
例如 IBookAppService 接口和 BookCreationDto 类都适合放在这个项目中.
	说明：它依赖 .Domain.Shared 因为它可能会在应用接口和DTO中使用常量,枚举和其他的共享对象.
- Application 项目
项目包含 .Application.Contracts 项目的 应用服务 接口实现.
例如 BookAppService 类适合放在这个项目中.
  - 它依赖 .Application.Contracts 项目, 因为它需要实现接口与使用DTO.
  - 它依赖 .Domain 项目,因为它需要使用领域对象(实体,仓储接口等)执行应用程序逻辑.
- .EntityFrameworkCore 项目
这是集成EF Core的项目. 它定义了 DbContext 并实现 .Domain 项目中定义的仓储接口.
	它依赖 .Domain 项目,因为它需要引用实体和仓储接口.
只有在你使用了EF Core做为数据库提供程序时,此项目才会可用. 如果选择的是其他数据库提供程序那么项目的名称会改变。
- HttpApi 项目
用于定义API控制器.
	大多数情况下,你不需要手动定义API控制器,因为ABP的动态API功能会根据你的应用层自动创建API控制器. 但是,如果你需要编写API控制器,那么它是最合适的地方.
	说明：它依赖 .Application.Contracts 项目,因为它需要注入应用服务接口.
- .HttpApi.Client 项目
定义C#客户端代理使用解决方案的HTTP API项目. 可以将上编辑共享给第三方客户端,使其轻松的在DotNet应用程序中使用你的HTTP API(其他类型的应用程序可以手动或使用其平台的工具来使用你的API).
	ABP有动态 C# API 客户端功能,所以大多数情况下你不需要手动的创建C#客户端代理.
	说明：它依赖 .Application.Contracts 项目,因为它需要使用应用服务接口和DTO.
如果你不需要为API创建动态C#客户端代理,可以删除此项目和依赖项
- HttpApi.Host 项目
	项目的启动项，包含应用程序主要的 appsettings.json 配置文件,用于配置数据库连接字符串和应用程序的其他配置等。


### 初始化项目数据库
> 认证服务（AuthServer.Host）
- 作用：token认证、服务间通信认证等,认证服务：AuthServer.Host,   首先，在appsettings.json中配置好数据库连接，比如：
 ``` xml
"Default": "Server=localhost;Database=ABP;Trusted_Connection=True;TrustServerCertificate=True;"
 ```
- 示例图
   > ![image](https://github.com/user-attachments/assets/db8005c6-7302-4df9-be62-ae4371e6ad4e)

- 设置AuthServer.Host为启动项目，执行迁移命令：Update-database
   > ![image](https://github.com/user-attachments/assets/0280df42-5545-43c8-b130-88a1f681a599)

- 自动生成数据库相关认证的表：
     ![image](https://github.com/user-attachments/assets/db66af7b-4310-4281-b73f-fd5727df7925)
> 基础服务BaseService
- 作用：系统管理功能，包括：用户管理、角色管理、菜单管理、租户管理、数据字典等系统基础功能
- 配置数据库连接：
  > ![image](https://github.com/user-attachments/assets/2c440f00-bc11-49f5-b6d1-fcf20d1dde4e)

- 执行数据库迁移
  > ![image](https://github.com/user-attachments/assets/93f8f503-8e5d-47a9-9488-7b8cd9686431)

- 生成数据库表：
  > ![image](https://github.com/user-attachments/assets/36283e44-5758-4b87-8b09-aab20f038db1)

- 其他项目说明
  > ![image](https://github.com/user-attachments/assets/27aa60bf-e5e1-40c7-bad1-2791bd233a6d)

 - 文件服务：FileStorage.Host（开发中）
 - 定时任务：HangFireJob.HttpApi.Host   定时与业务完全分离（完成）
 - WMS基础服务：WMS.BaseService.HttpApi.Host（例如：基础档案等一维表的基础服务）（准备开发中）
 - WMS主业务：WMS.Business.HttpApi.Host（主业务） 与基础服务通过 内部网关进行 服务间通信（准备开发中）

### 开发进度
1，WMS相关业务准备开发中，目前进度：架起了ABP微服务架构。
2，加入了前端vue-admin-element框架。

### 注意事项
开源项目，大家一学习应用！

### ABP微服务（学习文档）
编写中，将从零开始学习，DDD原则，包括以后会陆续出相关视频。
- 基础篇：框架和DDD
- 中级篇：分布式配置
- 高级篇：微服务
- 源码及环境安装
- 前端篇：Vue2.0
- 视频讲解

## License
遵守的协议
## 技术支持群
QQ:82032553
