using HangFireJob.EventArgs;
using HangFireJob.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace HangFireJob.Samples.ExportJob
{
    /// <summary>
    /// 具体实现
    /// </summary>
    public class HExportJob : ITransientDependency, IExportJob
    {
        private readonly IGuidGenerator _guidGenerator;

        public HExportJob(
            IGuidGenerator guidGenerator

            )
        {
            _guidGenerator = guidGenerator;

        }
        /// <summary>
        /// 执行单次任务时调用的方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exportName"></param>
        /// <param name="p_list"></param>
        /// <returns></returns>
        public async Task<string> ExportFile(Guid id, List<string> exportName, List<CustomCondition> p_list)
        {
            Console.WriteLine("红旗业务-单次任务测试-test");
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
