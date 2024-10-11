using ABP.Business.CAPToken;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.MultiTenancy;

namespace ABP.Business.Samples.Boms
{
    /// <summary>
    /// CAP订阅示例，使用CapSubscribe来标识即可,记录失败类型、消息id、失败的消息内容、失败的消息方法等等，方便后期手动处理
    /// </summary>
    public class CapSubscribeService :ApplicationService, ICapSubscribe
    {
        private readonly ICapPublisher _capPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBomAppService _bomAppService;
        public CapSubscribeService(ICapPublisher capPublisher,
            IBomAppService bomAppService,
            IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _capPublisher = capPublisher;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _bomAppService = bomAppService;
        }
        /// <summary>
        /// 发布消息示例
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TestPublish()
        {
            var headers = new Dictionary<string, string>()
            {
                ["my.header.id"] = Guid.NewGuid().ToString()
            };
            // Token
            //模拟接收访问接口时Header请求头中的Authorization存储的token
            if (!headers.ContainsKey("Authorization")
                && _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                headers.Add("Authorization", _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString());
            }
            await _capPublisher.PublishAsync("，mr_testsubscribe", "hello：MR", headers);
            return (IActionResult)Task.CompletedTask;
        }

        /// <summary>
        /// 订阅示例
        /// </summary>
        /// <param name="body"></param>
        /// <param name="header"></param>
        [CapSubscribe("mr_testsubscribe")]
        private async void TestSubscribe2(string body, [FromCap] CapHeader header)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IBomAppService>();
                // 模拟HttpContext
                var mockHttpContext = new CapHttpContext(_serviceProvider).SetCAPHeader(header);
                mockHttpContext.MockToken();//接收token成功
                //如果租户没有切换，执行如下代码，进行强制切换
                CurrentTenant.Change(CurrentUser.TenantId);
                await _bomAppService.GetListAsync();//模拟调用业务服务代码
                Console.WriteLine($"已经订阅了消息：{body}");
                Console.WriteLine($"当前消息id{header["cap-msg-id"]}");
                Console.WriteLine($"当前消费时间：{header["cap-senttime"]}");
                //有这样的情景：订单业务中，当消费方法处理订单新增的业务已经成功了，但是写入日志的地方报错了，导致触发了重试机制一直进行请求消费
                //对于普通业务，可以记录消息id或者自定义的唯一键去处理，每次处理都查询这个消息id或者自定义的唯一键是否存在，再去处理,以实现CAP中的幂等性
                Console.WriteLine("header 中自定义传送的唯一键:" + header["my.header.id"]);
            }
        }
        /// <summary>
        /// CAP延时加载，CAP7.0版本后支持,对于延迟消息，Dashboard面板中将支持对延迟消息查看和操作
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TestPublishDelay()
        {
            Console.WriteLine($"已经发布延时消息，将在20秒之后消费，当前时间为{DateTime.Now}");
            await _capPublisher.PublishDelayAsync(TimeSpan.FromSeconds(20), "testdelaysubscribe", "hello");
            return (IActionResult)Task.CompletedTask;
        }

        [CapSubscribe("testsubscribe")]
        private void TestSubscribe(string body, [FromCap] CapHeader header)
        {
            Console.WriteLine($"已经订阅了消息：{body}");

            throw new Exception("手动抛出异常");
        }

        /// <summary>
        /// 发布死信队列监控
        /// </summary>
        /// <param name="body"></param>
        [CapSubscribe("publish-dead-letter-queue")]
        private void PublishDeadQueue(string body, [FromCap] CapHeader header)
        {
            Console.WriteLine("发布异常");
            Console.WriteLine($"进入了发布死信队列，消息内容：{body}");
            Console.WriteLine($"异常的消息id：{header["header.error.msgid"]}");
            Console.WriteLine($"异常的消息方法名：{header["header.error.msgname"]}");
            Console.WriteLine($"当前消费时间：{header["cap-senttime"]}");
            //写入数据库和日志


        }
        /// <summary>
        /// 订阅死信队列监控
        /// </summary>
        /// <param name="body"></param>
        [CapSubscribe("subscribe-dead-letter-queue")]
        private void SubscribeDeadQueue(string body, [FromCap] CapHeader header)
        {
            Console.WriteLine("订阅异常");
            Console.WriteLine($"进入了订阅死信队列，消息内容：{body}");
            Console.WriteLine($"异常的消息id：{header["header.error.msgid"]}");
            Console.WriteLine($"异常的消息方法名：{header["header.error.msgname"]}");
            Console.WriteLine($"当前消费时间：{header["cap-senttime"]}");
            //写入数据库和日志


        }

    }
}
