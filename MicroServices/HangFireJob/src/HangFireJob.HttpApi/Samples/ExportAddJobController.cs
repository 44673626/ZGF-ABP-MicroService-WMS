using Hangfire;
using HangFireJob.EventArgs;
using HangFireJob.IServices.Dto;
using HangFireJob.Samples.IReport.Export;
using HangFireJob.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.Samples
{
    [Route("api/hangfire/bj")]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ExportAddJobController : HangFireJobController, IHExportService
    {
        private readonly IHExportService _sampleAppService;

        public ExportAddJobController(IHExportService sampleAppService)
        {
            _sampleAppService = sampleAppService;
        }

        [HttpPost]
        [Route("AddBackJob-Schedule")]
        public async Task<string> AddScheduleJob(HttpJobDescriptorDto jobDescriptor)
        {
            return await _sampleAppService.AddScheduleJob(jobDescriptor);
        }
        /// <summary>
        /// 创建异步任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exportName"></param>
        /// <param name="p_list"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddBackJob")]
        public async Task<string> AddBackGroundJob(Guid id, List<string> exportName, List<CustomCondition> p_list)
        {
            var taksId = string.Empty;
            taksId = await _sampleAppService.AddBackGroundJob(id, exportName, p_list);
            return taksId;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteBackJob/{jobId}")]
        public async Task DeleteJob(string jobId)
        {
            await _sampleAppService.DeleteJob(jobId);
        }

        [HttpPost]
        [Route("RequeueBackJob/{jobId}")]
        public Task<bool> Requeue(string jobId)
        {
            return _sampleAppService.Requeue(jobId);
        }

    }
}
