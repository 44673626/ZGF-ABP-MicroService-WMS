using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Win.Sfs.FileStorage.UploadFile;
using Win.Sfs.FileStorage.UploadFile.Dto;
using Shouldly;
using FileStorage.ImportMap;

namespace FileStorage.Common
{
    public class ExportImporter
    {

        private readonly IExporter _csvExporter = new Magicodes.ExporterAndImporter.Csv.CsvExporter();//导出CSV
        private readonly IExcelImporter _importer = new Magicodes.ExporterAndImporter.Excel.ExcelImporter();//导入Excel
        private readonly IExporter _exporter = new ExcelExporter();//导出Excel

        public virtual async Task<List<T>> UploadExcelImport<T>([FromForm] IFormFileCollection files, IFileStorageBlobAppService _excelImportService)
    where T : class, new()
        {
            Type type = typeof(T).GetType();
            var ImportList = new List<T>();
            ExcelImportResult returnResult = new ExcelImportResult();
            List<string> _errorList = new List<string>();

            var importAttrib = type.GetCustomAttribute<ImportProjectAttribute>();
            if (importAttrib != null)
            {

                foreach (var itm in type.GetProperties())
                {
                    var attrib = itm.GetCustomAttribute<ImporterHeaderAttribute>();
                    if (attrib != null)
                    {
                        // attrib.Name
                    }
                }
            }
            foreach (var file in files)
            {
                if (file == null)
                {
                    throw new BusinessException("上传附件不能为空！");
                }

                string FileOriginName = file.FileName;
                string getFileName = Path.GetFileName(FileOriginName);//获取附件名称
                using (var memoryStream = new MemoryStream())
                {
                    //保存成物理文件
                    await file.CopyToAsync(memoryStream);
                    await _excelImportService.SaveBlobAsync(
                             new SaveFileBlobInputDto
                             {
                                 Name = Path.GetFileName(FileOriginName),
                                 Content = memoryStream.ToArray()
                             }
                         );
                }
                //读取文件保存的根目录
                string fileSaveRootDir = ConfigDirHelper.GetAppSetting("App", "FileRootPath");
                //读取WMS文件保存的模块的根目录
                string fileSaveDir = ConfigDirHelper.GetAppSetting("App", "WMSFiles");
                //文件保存的相对文件夹(保存到wwwroot目录下)
                string absoluteFileDir = fileSaveRootDir + @"\" + fileSaveDir;
                //文件保存的路径(应用的工作目录+文件夹相对路径);
                string fileSavePath = Environment.CurrentDirectory + @"\wwwroot\host\my-file-container" + absoluteFileDir;
                var filePath = fileSavePath + getFileName;//获取到导入的excel
                var import = await _importer.Import<T>(filePath);
                if (import.Exception != null)
                {
                    if (import.Exception.Message.ToString() == "导入文件不存在!")
                    {
                        throw new BusinessException("8989", "文件容器配置的路径错误,请检查！");
                    }
                    else
                    {
                        throw new BusinessException("8989", import.Exception.Message.ToString());
                    }
                }
                else
                {
                    if (import.TemplateErrors.Count > 0)
                    {
                        throw new BusinessException("8989", "模板错误！当前模板中字段不匹配！！请正确上传模板数据！导入不对的列名：" + import.TemplateErrors.FirstOrDefault().RequireColumnName);
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
                        foreach (var itm in import.RowErrors)
                        {
                            string error = string.Empty;
                            foreach (var dic in itm.FieldErrors)
                            {
                                error += (dic.Key + dic.Value + "|");
                            }

                            _errorList.Add(string.Format("第{0}行{1}", itm.RowIndex.ToString(), error));
                        }
                        //自动保存“{ 目标文件名称}_.xlsx”的标注文件到目标位置
                        //导入的数据有错误，比如重复列，必填项为空等（不包含警告），主要看DTO的设置
                        import.HasError.ShouldBeTrue();//标识导入的数据有错误
                        returnResult.errSize = import.RowErrors.Count;
                        returnResult.errTemplate = Path.GetFileNameWithoutExtension(FileOriginName) + "_.xlsx";//返回错误模板名称作为参数
                        returnResult.errMessage = "RowErrors";
                    }
                    else
                    {
                        import.HasError.ShouldBeFalse();//标识导入的数据没有错误了
                        if (import.Data.Count > 0)
                        {
                            ImportList.AddRange(import.Data.ToList());
                        }
                        returnResult.errMessage = "SccessData";
                    }
                }
            }
            if (_errorList.Count > 0)
            {
                throw new BusinessException("8989", string.Join("\r\n", _errorList));
            }
            return ImportList;//返回客户端
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



        public virtual async Task<List<T>> ExtendExcelImport<T>([FromForm] IFormFileCollection files, IFileStorageBlobAppService _excelImportService, List<ImportColumnMap> p_list = null)
        where T : class, new()
        {

            Type type = typeof(T);
            var ImportList = new List<T>();
            ExcelImportResult returnResult = new ExcelImportResult();


            List<string> _errorList = new List<string>();
            foreach (var file in files)
            {
                if (file == null)
                {
                    throw new BusinessException("上传附件不能为空！");
                }
                string FileOriginName = file.FileName;
                string getFileName = Path.GetFileName(FileOriginName);//获取附件名称
                using (var memoryStream = new MemoryStream())
                {
                    //保存成物理文件
                    await file.CopyToAsync(memoryStream);
                    await _excelImportService.SaveBlobAsync(
                             new SaveFileBlobInputDto
                             {
                                 Name = Path.GetFileName(FileOriginName),
                                 Content = memoryStream.ToArray()
                             }
                         );
                }
                //读取文件保存的根目录
                string fileSaveRootDir = ConfigDirHelper.GetAppSetting("App", "FileRootPath");
                //读取WMS文件保存的模块的根目录
                string fileSaveDir = ConfigDirHelper.GetAppSetting("App", "WMSFiles");
                //文件保存的相对文件夹(保存到wwwroot目录下)
                string absoluteFileDir = fileSaveRootDir + @"\" + fileSaveDir;
                //文件保存的路径(应用的工作目录+文件夹相对路径);
                string fileSavePath = Environment.CurrentDirectory + @"\wwwroot\files\host\my-file-container" + absoluteFileDir;
                var filePath = fileSavePath + getFileName;//获取到导入的excel

                //ExcelHelper _excelHelper = new ExcelHelper(filePath);
                //if (p_list != null && p_list.Count > 0)
                //{
                //    ImportList = _excelHelper.ExcelToListByMap<T>(p_list);
                //}
                //else
                //{
                //    ImportList = _excelHelper.ExcelToList<T>();
                //}

            }
            return ImportList;//返回客户端
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
