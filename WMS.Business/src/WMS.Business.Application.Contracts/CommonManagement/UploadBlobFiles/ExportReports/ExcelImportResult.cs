using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.CommonManagement.UploadBlobFiles.ExportReports
{
    public class ExcelImportResult
    {
        /// <summary>
        /// 导入的数据总数
        /// </summary>
        public long totalSize { get; set; }

        /// <summary>
        /// 成功的个数
        /// </summary>
        public long succeessSize { get; set; }

        /// <summary>
        /// 错误数据个数
        /// </summary>
        public long errSize { get; set; }
        /// <summary>
        /// 导入错误生成的模板名称
        /// </summary>
        public string errTemplate { get; set; }

        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string errMessage { get; set; }
    }
}
