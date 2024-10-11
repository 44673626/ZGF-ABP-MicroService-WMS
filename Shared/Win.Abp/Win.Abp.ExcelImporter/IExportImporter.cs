using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Win.Sfs.Shared.ApplicationBase;

namespace Win.Abp.ExcelImporter
{
   public interface IExportImporter: ITransientDependency 
    {
        /// <summary>
        /// 生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<byte[]> GetExcelImportTemplate<T>() where T : class, new();
        /// <summary>
        /// 导入接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="files"></param>
        /// <param name="cacheService"></param>
        /// <returns></returns>
        Task<ExcelImportResult> UploadExcelImport<T>([FromForm] IFormFileCollection files, IImportAppService<T> cacheService)
            where T : class, new();
        /// <summary>
        /// 导出接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dots"></param>
        /// <returns></returns>
        Task<byte[]> ExcelExporter<T>(List<T> dots) where T : class, new();
    }
}
