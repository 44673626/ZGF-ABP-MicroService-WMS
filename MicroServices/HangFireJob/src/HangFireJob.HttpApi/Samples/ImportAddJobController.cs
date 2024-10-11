using HangFireJob.EventArgs;
using HangFireJob.Samples.IReport.Import;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFireJob.Samples
{
    /// <summary>
    /// 导入功能
    /// </summary>
    [Route("api/hangfire/import")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ImportAddJobController : HangFireJobController, IImportService
    {
        private readonly IImportService _sampleAppService;

        public ImportAddJobController(IImportService sampleAppService)
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
        [Route("AddBackJob")]
        public async Task<string> AddBackGroundJob(Guid id, List<string> exportName, List<string> realfileName, List<CustomCondition> p_list)
        {
            var taksId = string.Empty;
            taksId = await _sampleAppService.AddBackGroundJob(id, exportName, realfileName, p_list);
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
