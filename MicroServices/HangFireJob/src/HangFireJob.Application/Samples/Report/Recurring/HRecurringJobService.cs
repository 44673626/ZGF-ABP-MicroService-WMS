using Hangfire;
using HangFireJob.EventArgs;
using HangFireJob.IServices.Dto;
using HangFireJob.Samples.ExportJob;
using HangFireJob.Samples.IReport.Recurring;
using HangFireJob.Services.Common;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.Report.Recurring
{
    /// <summary>
    /// 定时任务执行-样例
    /// </summary>
    public class HRecurringJobService : ITransientDependency, IRecurringJobService
    {
        //具体执行的任务
        private readonly HExportJob _job_hq;

        public HRecurringJobService(
            HExportJob job_hq
            )
        {
            _job_hq = job_hq;

        }

        #region 调用远端服务
        /// <summary>
        /// 添加一个定时任务
        /// </summary>
        /// <param name="jobDestriptor"></param>
        /// <returns></returns>
        //[HttpPost("AddRecurring")]
        public JsonResult Recurring([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            //// 使用Hangfire的RecurringJobSchedule方法设置Cron
            //RecurringJob.RemoveIfExists(jobDescriptor.JobName);
            //RecurringJob.AddOrUpdate(
            //    jobDescriptor.JobName,
            //    cronExpression: cronExpression
            //);
            try
            {
                var jobId = string.Empty;
                RecurringJob.AddOrUpdate(jobDescriptor.JobName, () => HttpJobExecutor.DoRequest(jobDescriptor), CronType.Minute(int.Parse(jobDescriptor.Cron)), TimeZoneInfo.Local);
                return new JsonResult(new { Flag = true, Message = $"Job:{jobDescriptor.JobName}已加入队列" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, ex.Message });
            }
        }
        [HttpPost("AddRecurring")]
        public async Task<string> AddOrUpdateRecurring([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            //任务ID
            var jobId = "job-recurring";
            try
            {
                RecurringJob.AddOrUpdate(jobDescriptor.JobName, () => HttpJobExecutor.DoRequest(jobDescriptor), CronType.Minute(int.Parse(jobDescriptor.Cron)), TimeZoneInfo.Local);
                //return new JsonResult(new { Flag = true, Message = $"Job:{jobDescriptor.JobName}已加入队列" });
            }
            catch (Exception ex)
            {
                // return new JsonResult(new { Flag = false, Message = ex.Message });
            }
            return jobId;
        }

        /// <summary>
        /// 添加一个延迟任务到队列
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        [HttpPost("AddSchedule")]
        public JsonResult Schedule([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            try
            {
                var jobId = string.Empty;
                jobId = Hangfire.BackgroundJob.Schedule(() => HttpJobExecutor.DoRequest(jobDescriptor), TimeSpan.FromMinutes((double)jobDescriptor.DelayInMinute));
                return new JsonResult(new { Flag = true, Message = $"Job:#{jobId}-{jobDescriptor.JobName}已加入队列" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, ex.Message });
            }
        }

        /// <summary>
        /// 删除一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRecurring")]
        public JsonResult Delete(string jobName)
        {
            try
            {
                RecurringJob.RemoveIfExists(jobName);
                return new JsonResult(new { Flag = true, Message = $"Job:{jobName}已删除" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, ex.Message });
            }
        }

        /// <summary>
        /// 触发一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpGet("TriggerRecurring")]
        public JsonResult Trigger(string jobName)
        {
            try
            {
                RecurringJob.Trigger(jobName);
                return new JsonResult(new { Flag = true, Message = $"Job:{jobName}已触发执行" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Flag = false, ex.Message });
            }

        }

        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        //[HttpGet("HealthCheck")]
        //public IActionResult HealthCheck()
        //{
        //    var serviceUrl = Request.Host;
        //    return new JsonResult(new { Flag = true, Message = "All is Well!", ServiceUrl = serviceUrl, CurrentTime = DateTime.Now });
        //}
        #endregion

        #region sample
        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <returns></returns>
        public async Task<string> AddOrUpdateRecurringJob(Guid id, List<string> exportName, List<CustomCondition> p_list)
        {
            //任务ID
            var jobId = "job-recurring";
            //创建异步任务
            //Hangfire 注册的时候默认是单例模式，所以在任意代码中使用其静态方法就能添加异步任务或者定时任务
            RecurringJob.AddOrUpdate(jobId, () => _job_hq.ExportFile(id, exportName, p_list), Cron.Minutely, TimeZoneInfo.Local);
            return jobId;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task RemoveIfExistsJob(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        /// <summary>
        /// 立即触发任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task TriggerJob(string jobId)
        {
            RecurringJob.Trigger(jobId);
        }
        #endregion
    }
}
