using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Core.Extension;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Validation;
using Win.Sfs.Shared.ApplicationBase;

namespace Win.Abp.ExcelImporter
{
    public class ExportImporter : IExportImporter
    {
        private readonly IExcelImporter _importer = new Magicodes.ExporterAndImporter.Excel.ExcelImporter();//导入Excel
        private readonly IExporter _exporter = new ExcelExporter();//导出Excel

        /// <summary>
        /// 生成导入模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual async Task<byte[]> GetExcelImportTemplate<T>() where T : class, new()
        {
            Type type = typeof(T).GetType();
            var result = await _importer.GenerateTemplateBytes<T>();
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            Stream stream = new MemoryStream(result);
            using (var pck = new ExcelPackage(stream))
            {
                pck.Workbook.Worksheets.Count.ShouldBe(1);
                var sheet = pck.Workbook.Worksheets.First();
                var attr = typeof(T).GetAttribute<ExcelImporterAttribute>();
                var text = sheet.Cells["A1"].Text.Replace("\n", string.Empty).Replace("\r", string.Empty);
                text.ShouldBe(attr.ImportDescription.Replace("\n", string.Empty).Replace("\r", string.Empty));
            }
            return result;
        }


        /// <summary>
        ///Excel导入功能 
        /// </summary>
        /// <param name="files">导入文件</param>
        /// <param name="cacheService">缓存层导入接口</param>
        /// <returns></returns>
        public virtual async Task<ExcelImportResult> UploadExcelImport<T>([FromForm] IFormFileCollection files, IImportAppService<T> cacheService)
    where T : class, new()
        {
            Type type = typeof(T).GetType();
            ExcelImportResult returnResult = new ExcelImportResult();
            foreach (var file in files)
            {
                if (file == null)
                {
                    throw new BusinessException("上传附件不能为空！");
                }
                if (file.Length > 20971520) //10MB = 1024 * 1024 *20
                {
                    throw new BusinessException("上传附件大小能超过20M！");
                }
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);//生成文件
                    #region 导入
                    var import = await _importer.Import<T>(memoryStream);
                    //if (import.Exception != null)
                    //{
                    //    if (import.Exception.Message.ToString() == "导入文件不存在!")
                    //    {
                    //        throw new BusinessException("文件容器配置的路径错误,请检查！");
                    //    }
                    //}
                    if (import.TemplateErrors.Count > 0)
                    {
                        throw new BusinessException("模板错误！当前模板中字段不匹配！！请正确上传模板数据！");
                    }
                    import.ShouldNotBeNull();
                    if (import.Exception != null)
                    {
                        //导入的数据有异常
                        throw new BusinessException(import.Exception.ToString());
                    }
                    returnResult.totalSize = import.Data.Count;
                    if (import.RowErrors.Count > 0)
                    {
                        //--注意：文件流的方式不支持生成错误模板了
                        //自动保存“{ 目标文件名称}_.xlsx”的标注文件到目标位置
                        //导入的数据有错误，比如重复列，必填项为空等（不包含警告），主要看DTO的设置
                        //import.HasError.ShouldBeTrue();//标识导入的数据有错误
                        //returnResult.errSize = import.RowErrors.Count;
                        //returnResult.errTemplate = Path.GetFileNameWithoutExtension(FileOriginName) + "_.xlsx";//返回错误模板名称作为参数
                        //returnResult.errMessage = "RowErrors";
                        //错误列表，未测试
                        throw new AbpValidationException("err!",
                            new List<ValidationResult>
                            {
                                new ValidationResult("err!", new[] {"err"})
                            });
                    }
                    else
                    {
                        import.HasError.ShouldBeFalse();//标识导入的数据没有错误了
                        if (import.Data.Count > 0)
                        {
                            //获取导入的Excel中的数据列表
                            var customerImportList = import.Data.ToList();
                            await cacheService.ImportAsync(customerImportList);
                        }
                        else
                        {
                            throw new BusinessException("模板数据为空！请检查！");
                        }
                    }
                    #endregion
                }
            }

            return returnResult;//返回客户端
        }

        /// <summary>
        /// 导出Excel文件公用方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dots"></param>
        /// <returns></returns>
        public virtual async Task<byte[]> ExcelExporter<T>(List<T> dots) where T : class, new()
        {
            var result = await _exporter.ExportAsByteArray(dots);
            return result;
        }


    }

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
