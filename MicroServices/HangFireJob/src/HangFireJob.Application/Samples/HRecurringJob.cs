using HangFireJob.EventArgs;
using HangFireJob.IServices;
using HangFireJob.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace HangFireJob.Samples
{
    /// <summary>
    /// 定时任务要执行的具体任务--异步
    /// </summary>
    public class HRecurringJob : ITransientDependency, IExportJob
    {
        private readonly IGuidGenerator _guidGenerator;
        public HRecurringJob(
            IGuidGenerator guidGenerator
            )
        {
            _guidGenerator = guidGenerator;
        }
        public async Task<string> ExportFile(Guid id, List<string> exportName, List<CustomCondition> p_list)
        {
            Console.WriteLine("红旗业务-定时任务测试-test");
            var guid = Guid.NewGuid();
            List<CustomCondition> customConditionList = new List<CustomCondition>();
            customConditionList.Add(new CustomCondition() { Name = "Version", Value = "202306" });
            customConditionList.Add(new CustomCondition() { Name = "MaterialCode", Value = "123456" });
            List<string> export_name = new List<string>();
            export_name.Add("红旗后台任务");
            //TOOD 其他实现
            return id.ToString();
        }
    }
}
