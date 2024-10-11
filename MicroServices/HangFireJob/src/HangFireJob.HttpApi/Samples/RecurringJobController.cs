using HangFireJob.EventArgs;
using HangFireJob.IServices.Dto;
using HangFireJob.Samples.IReport.Recurring;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.Samples
{
    /// <summary>
    /// hangfire定时任务-可用于同步接口数据操作
    /// </summary>
    [Route("api/hangfire/recurring")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class RecurringJobController : HangFireJobController, IRecurringJobService
    {
        private readonly IRecurringJobService _sampleAppService;

        public RecurringJobController(IRecurringJobService sampleAppService)
        {
            _sampleAppService = sampleAppService;
        }
        /// <summary>
        /// 创建异步任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exportName"></param>
        /// <param name="p_list"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddOrUpdateJob")]
        public async Task<string> AddOrUpdateRecurringJob(Guid id, List<string> exportName, List<CustomCondition> p_list)
        {
            var taksId = string.Empty;
            taksId = await _sampleAppService.AddOrUpdateRecurringJob(id, exportName, p_list);
            return taksId;
        }

        [HttpPost]
        [Route("AddOrUpdate-Job")]
        public async Task<string> AddOrUpdateRecurring([FromBody] HttpJobDescriptorDto jobDescriptor)
        {
            return await _sampleAppService.AddOrUpdateRecurring(jobDescriptor);
            //return  new JsonResult(new { Flag = true, Message = $"Job:#{jobId}-{jobDescriptor.JobName}已加入队列" });
        }


        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveIfExistsJob/{jobId}")]
        public Task RemoveIfExistsJob(string jobId)
        {
            return _sampleAppService.RemoveIfExistsJob(jobId);
        }

        [HttpPost]
        [Route("TriggerJob/{jobId}")]
        public Task TriggerJob(string jobId)
        {
            return _sampleAppService.TriggerJob(jobId);
        }

    }
}
