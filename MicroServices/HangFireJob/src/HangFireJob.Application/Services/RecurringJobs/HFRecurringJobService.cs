using Cronos;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Storage;
using HangFireJob.IServices;
using HangFireJob.IServices.Dto;
using HangFireJob.Samples;
using HangFireJob.Services.Common;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;


namespace HangFireJob.Services.RecurringJobs
{
    /// <summary>
    /// 定时任务（循环执行）
    /// </summary>
    public class HFRecurringJobService : ApplicationService, IHFRecurringJobService
    {
        private readonly IRepository<HttpJobDescriptor, Guid> _repository;
        public HFRecurringJobService(IRepository<HttpJobDescriptor, Guid> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 生成基本的Cron表达式
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        public async Task<string> CreateCronGenerator(CronTypeDto cron)
        {
            var cronInfo = string.Empty;
            switch (cron.CronType)
            {
                case CronStateEnum.Minute:
                    cronInfo = CronType.Minute(cron.Interval);
                    break;
                case CronStateEnum.Hour:
                    cronInfo = CronType.Hour(cron.Minute, cron.Interval);
                    break;
                case CronStateEnum.Day:
                    cronInfo = CronType.Day(cron.Hour, cron.Minute, cron.Interval);
                    break;
                case CronStateEnum.Week:
                    cronInfo = CronType.Week(cron.Week, cron.Hour, cron.Minute);
                    break;
                case CronStateEnum.Month:
                    cronInfo = CronType.Month(cron.Day, cron.Hour, cron.Minute);
                    break;
                case CronStateEnum.Year:
                    cronInfo = CronType.Year(cron.Month, cron.Day, cron.Hour, cron.Minute);
                    break;
                default:
                    cronInfo = CronType.Minute(cron.Interval);
                    break;

            }
            return cronInfo;//返回cron表达式
        }

        /// <summary>
        /// 查询所有定时任务列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HttpJobDescriptorDto>> GetAllRecurringJobsList(string? jobName)
        {
            var getRecurringJobs = (await _repository.GetQueryableAsync()).Where(p => p.JobType == ((int)HangFireJobTypeEnum.RecurringJobs).ToString()).ToList();//过滤定时任务列表
            var dtos = ObjectMapper.Map<List<HttpJobDescriptor>, List<HttpJobDescriptorDto>>(getRecurringJobs);
            if (dtos.Count > 0)
            {
                var jobList = GetAllRecurringJobs();
                var _query = from job in dtos
                             join hangfirejob in jobList
                             on job.JobName equals hangfirejob.Id into tempJob
                             from hangfire in tempJob.DefaultIfEmpty()
                             where hangfire != null
                             select new HttpJobDescriptorDto()
                             {
                                 Id = job.Id,//主键ID
                                 HttpUrl = job.HttpUrl,
                                 JobId = string.IsNullOrEmpty(hangfire.LastJobId) ? 0 : int.Parse(hangfire.LastJobId),//最近执行的任务的ID
                                 JobName = job.JobName,
                                 JobType = ((int)HangFireJobTypeEnum.RecurringJobs).ToString(),//标识定时任务
                                 StateName = string.IsNullOrEmpty(hangfire.LastJobId) ? "任务新建,启动中" : hangfire.LastJobState,
                                 HttpMethod = job.HttpMethod,//参数类型
                                 Cron = job.Cron,
                                 LastJobState = hangfire.LastJobState,//最近一次运行状态
                                 LastExecution = hangfire.LastExecution.HasValue ?
                                    TimeZoneInfo.ConvertTimeFromUtc(hangfire.LastExecution.Value, TimeZoneInfo.Local) : null,
                                 //LastExecution = CronExpression.Parse(job.Cron).GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local)?.DateTime,
                                 //LastExecution = hangfire.LastExecution,//最近一次运行时间,用的是协调世界时（UTC）时间，不受时区的影响
                                 CreationTime = job.CreationTime,//创建时间
                                 Remark = job.Remark,//任务描述
                                 TimeSpanFromSeconds = job.TimeSpanFromSeconds,//任务执行的间隔时间
                             };
                dtos = _query.WhereIf(!string.IsNullOrEmpty(jobName), p => p.JobName.Contains(jobName)).ToList();
            }
            return dtos;
        }

        /// <summary>
        /// 获取所有定时任务列表（从Hangfire中查询）
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private List<RecurringJobDto> GetAllRecurringJobs()
        {
            var jobList = new List<RecurringJobDto>();
            try
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    jobList = connection.GetRecurringJobs();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(message: "HttpJobDispatcher.GetAllRecurringJobs");
            }
            return jobList;
        }

        /// <summary>
        /// 创建或修改定时任务
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateRecurring([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            var jobName = jobDescriptor.JobName;
            var expressionStr = string.Empty;
            var exist = await _repository.FirstOrDefaultAsync(_ => _.JobName == jobName);
            if (exist != null)
            {
                throw new BusinessException(message: "任务名称：" + jobName + "已存在，不能重复！");
            }
            if (string.IsNullOrEmpty(jobDescriptor.Cron))
                throw new BusinessException(message: "间隔时间即Cron表达式不能为空！");
            var entity = ObjectMapper.Map<HttpJobDescriptorDto, HttpJobDescriptor>(jobDescriptor);
            entity.JobName = jobName;//任务名称
            entity.JobType = ((int)HangFireJobTypeEnum.RecurringJobs).ToString();//标识定时任务                                                                  
            bool hasSeconds = CheckIfCronExpressionHasSeconds(jobDescriptor.Cron);// 检查是否包含秒字段
            var now = DateTime.UtcNow;
            if (!hasSeconds)
            {
                var expression = CronExpression.Parse(jobDescriptor.Cron);//不含秒
                var nextUtc = expression.GetNextOccurrence(now);
                var span = nextUtc - now;
                if (span != null && (int)span.Value.TotalMilliseconds > 0)
                {
                    entity.TimeSpanFromSeconds = (int)span.Value.TotalMilliseconds;//间隔时间：秒
                }
                expressionStr = expression.ToString();
            }
            else
            {
                var expression = CronExpression.Parse(jobDescriptor.Cron, CronFormat.IncludeSeconds);//秒级
                var nextUtc = expression.GetNextOccurrence(now);
                var span = nextUtc - now;
                if (span != null && (int)span.Value.TotalMilliseconds > 0)
                {
                    entity.TimeSpanFromSeconds = (int)span.Value.TotalMilliseconds;//间隔时间：秒
                }
                expressionStr = expression.ToString();
            }
            await _repository.InsertAsync(entity);//往调度表中添加数据
            //var localUtc = new RecurringJobOptions { TimeZone = TimeZoneInfo.Local };
            RecurringJob.AddOrUpdate(jobName, () => HttpJobExecutor.DoRequest(jobDescriptor), expressionStr, TimeZoneInfo.Local);//其中Cron为cron表达式
            return true;

        }
        /// <summary>
        /// 判断是否含秒
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        private static bool CheckIfCronExpressionHasSeconds(string cronExpression)
        {
            // 移除所有空格并分割字段
            string[] fields = cronExpression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // 检查字段数量
            if (fields.Length == 6)
            {
                // 进一步验证秒字段是否为数字
                if (int.TryParse(fields[0], out int seconds) && seconds >= 0 && seconds <= 59)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 删除一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string jobName)
        {
            var getData = (await _repository.GetQueryableAsync()).FirstOrDefault(x => x.JobName == jobName);
            if (getData != null)
            {
                await _repository.DeleteAsync(getData.Id);//往调度表中删除数据
            }
            RecurringJob.RemoveIfExists(jobName);//删除任务
            return true;

        }

        /// <summary>
        /// 立即触发一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<bool> Trigger(string jobName)
        {
            RecurringJob.Trigger(jobName);
            return true;
        }


    }
}
