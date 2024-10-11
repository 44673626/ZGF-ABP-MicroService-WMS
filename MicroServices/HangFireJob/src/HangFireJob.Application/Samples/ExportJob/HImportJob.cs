using HangFireJob.EventArgs;
using HangFireJob.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace HangFireJob.Samples.ExportJob
{
    /// <summary>
    /// 导入-大数据
    /// </summary>
    public class HImportJob : ITransientDependency, IImportJob
    {

        //private readonly InvoiceVersionRepository _versionRepository;
        private readonly IGuidGenerator _guidGenerator;

        public HImportJob(
            IGuidGenerator guidGenerator
            //InvoiceVersionRepository versionRepository

            )
        {
            //_versionRepository = versionRepository;
            _guidGenerator = guidGenerator;
        }
        public string ImportFile(Guid taskid, List<string> fileName, List<string> realfileName, List<CustomCondition> customConditions)
        {
            string fileSavePath = Environment.CurrentDirectory + @"\wwwroot\files\host\my-file-container\";
            //var version = customConditions.Where(p => p.Name == "Version").FirstOrDefault().Value;
            //var customerCode = customConditions.Where(p => p.Name == "CustomerCode").FirstOrDefault().Value;
            //var year = customConditions.Where(p => p.Name == "Year").FirstOrDefault().Value;
            //var factory = customConditions.Where(p => p.Name == "Factory").FirstOrDefault().Value;
            var _id = _guidGenerator.Create();
            //TOOD 业务代码

            return taskid.ToString();
        }
    }
}
