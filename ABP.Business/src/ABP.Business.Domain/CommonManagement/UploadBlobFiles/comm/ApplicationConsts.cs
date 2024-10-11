using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABP.Business.CommonManagement.UploadBlobFiles.comm
{
    public static class ApplicationConsts
    {
        /// <summary>
        /// 导出文件
        /// </summary>
        public const string DefaultExportFileName = "导出文件";

        /// <summary>
        /// Success
        /// </summary>
        public const string SuccessStr = "Success";

        /// <summary>
        /// 文件后缀
        /// </summary>
        public const string FileExtension = ".xlsx";

        public static Guid TestGuid = new Guid("7CD45A6D-762A-6F0F-B9ED-39F91EEA93D1");

        /// <summary>
        /// 校验错误信息汇总表
        /// </summary>
       // public const string CheckErroFileName = "校验错误信息汇总表";
        public const string CheckErroFileName = "ErrorVerification";



    }
}
