using HangFireJob.EventArgs;
using HangFireJob.IServices.Dto;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace HangFireJob.Samples.IReport.Recurring
{
    /// <summary>
    /// hangfire-定时任务
    /// </summary>
    public interface IRecurringJobService : ITransientDependency
    {
        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <returns></returns>
        Task<string> AddOrUpdateRecurringJob(Guid id, List<string> exportName, List<CustomCondition> p_list);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task RemoveIfExistsJob(string jobId);

        /// <summary>
        /// 立即触发任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task TriggerJob(string jobId);


        Task<string> AddOrUpdateRecurring([FromBody] HttpJobDescriptorDto jobDescriptor);
    }
}
