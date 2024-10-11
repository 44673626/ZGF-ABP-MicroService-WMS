using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles.comm
{
    public static class ExcelCommonMethod
    {
        /// <summary>
        /// 获取单号
        /// </summary>
        /// <param name="header"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetDocumentNumber(string header, long num)
        {
            return $"{header}_{DateTime.Now.ToString("yyyyMMdd")}_{num}";
        }


        /// <summary>
        /// 获得导出文件名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="version"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetExcelFileName(string fileName, string version, string fileExtension)
        {
            return $"{fileName}_{version}{fileExtension}";
        }



        /// <summary>
        /// 获得导出文件名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="userId"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetExcelFileNameByUserID(string fileName, string userId, string fileExtension)
        {
            return $"{fileName}_{userId}{fileExtension}";
        }

    }
}
