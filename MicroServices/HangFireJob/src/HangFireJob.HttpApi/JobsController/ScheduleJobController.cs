using HangFireJob.IServices;
using HangFireJob.IServices.Dto;
using HangFireJob.Samples.IReport.Export;
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
    /// 单次任务，一次性“消费”
    /// </summary>
    [Route("api/hangFire/backGroupJobs")]
    public class ScheduleJobController : HangFireJobController, IHFScheduleJobService
    {
        private readonly IHFScheduleJobService _scheduleService;

        public ScheduleJobController(IHFScheduleJobService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        /// <summary>
        /// 单次任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("BackGroundJob-List")]
        public async Task<List<HttpJobDescriptorDto>> GetHttpbackgroundjobList(string jobName)
        {
            return await _scheduleService.GetHttpbackgroundjobList(jobName);
        }

        /// <summary>
        /// 添加后台任务
        /// </summary>
        /// <param name="jobDescriptor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BackGroundJob-Add")]
        public async Task<string> AddHttpbackgroundjob([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            return await _scheduleService.AddHttpbackgroundjob(jobDescriptor);
        }

        /// <summary>
        /// 删除单次任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BackGroundJob-Delete")]
        public async Task<bool> DeleteJob(string jobId)
        {
            return await _scheduleService.DeleteJob(jobId);
        }

        /// <summary>
        /// 重新执行队列
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BackGroundJob-Requeue")]
        public async Task<bool> Requeue(string jobId)
        {
            return await _scheduleService.Requeue(jobId);
        }


    }
}
