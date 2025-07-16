using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Guids;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Validation;
using Volo.Abp;
using WMS.BaseService.Logs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;
using WMS.BaseService.Logs;
using StackExchange.Profiling;

namespace WMS.BaseService.CommonManagement.AbpExceptionFilters
{
    public class EapExceptionFilter : IFilterMetadata, IAsyncExceptionFilter, ITransientDependency
    {
        public ILogger<EapExceptionFilter> Logger { get; set; }

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly AbpExceptionHandlingOptions _exceptionHandlingOptions;
        private readonly IRepository<AbpLogInfo, Guid> _logRepository;
        private readonly IGuidGenerator _guidGenerator;

        public EapExceptionFilter(
            IExceptionToErrorInfoConverter errorInfoConverter,
            IHttpExceptionStatusCodeFinder statusCodeFinder,
            IJsonSerializer jsonSerializer,
            IRepository<AbpLogInfo, Guid> logRepository,
            IGuidGenerator guidGenerator,
            IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
        {
            _errorInfoConverter = errorInfoConverter;
            _statusCodeFinder = statusCodeFinder;
            _jsonSerializer = jsonSerializer;
            _exceptionHandlingOptions = exceptionHandlingOptions.Value;

            Logger = NullLogger<EapExceptionFilter>.Instance;
            _logRepository = logRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }

            await HandleAndWrapException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            //if (context.ActionDescriptor.IsControllerAction()
            //         &&
            //         context.ActionDescriptor.HasObjectResult()
            //        )
            //{
            //    return true;
            //}
            //接所有异常
            if (!string.IsNullOrEmpty(context.Exception.Message))
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapException(ExceptionContext context)
        {
            //TODO: Trigger an AbpExceptionHandled event or something like that.
            var _id = _guidGenerator.Create();
            //添加请求头标识_AbpErrorFormat(给告诉调用者，这次的异常已经是被我们格式化的)
            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            //设置返回状态码
            context.HttpContext.Response.StatusCode = (int)_statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);

            //通过格式化转换器，将异常信息转换成为前端展示数据(这里就会使用到我们的配置信息)
            var remoteServiceErrorInfo = _errorInfoConverter.Convert(context.Exception, _exceptionHandlingOptions.SendExceptionsDetailsToClients);
            remoteServiceErrorInfo.Code = context.HttpContext.TraceIdentifier;
            remoteServiceErrorInfo.Message = SimplifyMessage(context, _id);

            //context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));//原代码
            //公司定义的返回前端VUE的消息格式
            context.Result = new JsonResult(new MessageModel
            {
                success = false,
                code = context.HttpContext.Response.StatusCode.ToString(),//错误码，比如403
                msg = remoteServiceErrorInfo.Message,//自定义信息
                msgDev = context.Exception.Message,
                response = null
            });
            //接口性能分析面板中，给出异常信息提示
            MiniProfiler.Current.Step("拦截的异常信息");
            MiniProfiler.Current.CustomTiming("Error:", context.Exception.Message);

            // 获取日志信息
            var logLevel = context.Exception.GetLogLevel();
            //创建一个StringBuilder对象拼接异常信息
            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(_jsonSerializer.Serialize(remoteServiceErrorInfo, indented: true));
            Logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());
            Logger.LogException(context.Exception, logLevel);
            //--写入数据库
            var logsql = new AbpLogInfo()
            {
                Message = remoteServiceErrorInfo.Message,
                MessageTemplate = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).DisplayName,//定位到控制器的错误方法
                Level = logLevel.ToString(),//日志级别
                TimeStamp = DateTime.Now,
                Exception = context.Exception.ToString(),//详细异常信息
                Properties = _jsonSerializer.Serialize(remoteServiceErrorInfo, indented: true)
            };
            logsql.SetValue(_id);
            await _logRepository.InsertAsync(logsql, true);
            //--结束写入数据库
            //获取注入IExceptionNotifier接口的实现类，给IExceptionSubscriber实现类接口批量发送事件
            await context.HttpContext
                .RequestServices
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(
                    new ExceptionNotificationContext(context.Exception)
                );
            //清空当前请求的异常
            context.Exception = null; //Handled!
        }

        /// <summary>
        /// 自定义异常信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        protected string SimplifyMessage(ExceptionContext context, Guid guid)
        {
            string message = string.Empty;
            switch (context.Exception)
            {
                case AbpAuthorizationException e:
                    context.HttpContext.Response.StatusCode = 401;
                    return message = "授权失败！";
                   case AbpValidationException e:
				   context.HttpContext.Response.StatusCode = 200;
				   if (e.ValidationErrors.Any())
				   {
					   message = "你的请求无效："+ string.Join(",", e.ValidationErrors);
				   }
				   else
				   {
					   message = e.ValidationErrors[0].ErrorMessage;

				   }
				   return message;
                case EntityNotFoundException e:
                    return message = "未找到对应实体！";
                case BusinessException e:
                    context.HttpContext.Response.StatusCode = 200;
                    return message = $"业务逻辑异常！{e.Message}";
                case NotImplementedException e:
                    return message = "not implement！";
                default:
                    context.HttpContext.Response.StatusCode = 200;
                    return message = $"服务器内部错误！错误码：{guid}";
            }
        }
    }
}
