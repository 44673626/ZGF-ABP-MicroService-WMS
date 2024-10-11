using HangFireJob.IServices.Dto;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace HangFireJob.IServices
{
    /// <summary>
    /// 执行定时任务
    /// </summary>
    public interface IHFRecurringJobService : IApplicationService
    {
        /// <summary>
        /// 生成cron表达式
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        Task<string> CreateCronGenerator(CronTypeDto cron);
        /// <summary>
        ///  创建或修改定时任务
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateRecurring([FromBody] HttpJobDescriptorDto jobDescriptor);

        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<bool> Delete(string jobName);

        /// <summary>
        /// 触发一个定时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<bool> Trigger(string jobName);
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        Task<List<HttpJobDescriptorDto>> GetAllRecurringJobsList(string? jobName);
    }
}
