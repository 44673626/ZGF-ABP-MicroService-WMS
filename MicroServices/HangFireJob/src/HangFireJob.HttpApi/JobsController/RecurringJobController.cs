using HangFireJob.IServices;
using HangFireJob.IServices.Dto;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.JobsController
{
    /// <summary>
    ///  定时执行任务
    /// </summary>
    [Route("api/hangFire/recurringJobs")]
    public class HXRecurringJobController : HangFireJobController, IHFRecurringJobService
    {
        private readonly IHFRecurringJobService _recurringService;

        public HXRecurringJobController(IHFRecurringJobService recurringService)
        {
            _recurringService = recurringService;
        }

        /// <summary>
        /// 生成基本的cron表达式
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("recurringJob-Cron")]
        public async Task<string> CreateCronGenerator(CronTypeDto cron)
        {
            return await _recurringService.CreateCronGenerator(cron);
        }

        /// <summary>
        /// 定时任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("recurringJob-List")]
        public async Task<List<HttpJobDescriptorDto>> GetAllRecurringJobsList(string? jobName)
        {
            return await _recurringService.GetAllRecurringJobsList(jobName);
        }

        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("recurringJob-Add")]
        public async Task<bool> AddOrUpdateRecurring([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            return await _recurringService.AddOrUpdateRecurring(jobDescriptor);
        }

        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recurringJob-Delete")]
        public async Task<bool> Delete(string jobName)
        {
            return await _recurringService.Delete(jobName);
        }

        /// <summary>
        /// 立即触发一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recurringJob-Trigger")]
        public async Task<bool> Trigger(string jobName)
        {
            return await _recurringService.Trigger(jobName);
        }
    }
}
