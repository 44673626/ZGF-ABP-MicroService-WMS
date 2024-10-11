using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application.Services;
using HangFireJob.Settings;
using System.Threading.Tasks;
using HangFireJob.IServices.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HangFireJob.IServices
{
    /// <summary>
    /// 单次任务，一次性“消费”
    /// </summary>
    public interface IHFScheduleJobService : IApplicationService
    {
        /// <summary>
        /// 后台任务列表
        /// </summary>
        /// <returns></returns>
        Task<List<HttpJobDescriptorDto>> GetHttpbackgroundjobList(string jobName);
        /// <summary>
        /// 添加后台任务
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        Task<string> AddHttpbackgroundjob([FromBody] HttpJobDescriptorDto jobDescriptor);
        /// <summary>
        /// 删除单次任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<bool> DeleteJob(string jobId);
        /// <summary>
        /// 重新加入对列
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<bool> Requeue(string jobId);
    }
}
