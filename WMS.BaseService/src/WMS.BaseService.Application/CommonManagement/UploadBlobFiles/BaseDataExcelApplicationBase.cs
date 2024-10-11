using WMS.BaseService.CommonManagement.UploadBlobFiles.comm;
using WMS.BaseService.CommonManagement.UploadBlobFiles.Dto;
using WMS.BaseService.CommonManagement.UploadBlobFiles.ExportReports;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles
{
    /// <summary>
    /// 封装文件导入及导出的通用方法
    /// </summary>
    /// <typeparam name="TCacheItem"></typeparam>
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public  class BaseDataExcelApplicationBase : ApplicationService//, ITransientDependency where TCacheItem : Entity
    {
        //protected IDistributedCache<TCacheItem> Cache { get; }
        //blob存储

        //public IFileStorageBlobAppService _fileStorageBlobAppService { get; set; }
        private readonly IFileStorageBlobAppService _fileStorageBlobAppService;
        private readonly IExcelImporter _importer = new ExcelImporter();//导入Excel类
        private readonly IExporter _exporter = new ExcelExporter();//导出Excel类

        public BaseDataExcelApplicationBase(IFileStorageBlobAppService fileStorageBlobAppService)
        {
            _fileStorageBlobAppService= fileStorageBlobAppService;
        }
        /// <summary>
        /// 导入文件(通用方法)
        /// </summary>
        /// <typeparam name="T">导入的泛型类（DTO）</typeparam>
        /// <param name="files">支持文件批量上传</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public virtual async Task<List<T>> UploadExcelImport<T>([FromForm] IFormFileCollection files)
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
                    throw new BusinessException(message: "上传附件不能为空！");
                }

                string FileOriginName = file.FileName;
                string getFileName = Path.GetFileName(FileOriginName);//获取附件名称
                using (var memoryStream = new MemoryStream())
                {
                    //保存成物理文件
                    await file.CopyToAsync(memoryStream);
                    await _fileStorageBlobAppService.SaveBlobAsync(
                        new SaveFileBlobInputDto
                        {
                            Name = Path.GetFileName(FileOriginName),
                            Content = memoryStream.ToArray()
                        }
                    );
                }
                //string fileSavePath = ConfigDirHelper.GetAppSetting("App", "FileRootPath");

                //文件保存的路径(应用的工作目录+文件夹相对路径);
                string fileSavePath = Environment.CurrentDirectory + @"\wwwroot\files\host\abp-file-container\";
                var filePath = fileSavePath + getFileName;//获取到导入的excel
                var import = await _importer.Import<T>(filePath);
                if (import.Exception != null)
                {
                    if (import.Exception.Message.ToString() == "导入文件不存在!")
                    {
                        throw new BusinessException(message: "文件容器配置的路径错误,请检查！");
                    }
                    else
                    {
                        throw new BusinessException(message: import.Exception.Message.ToString());
                    }
                }
                else
                {
                    if (import.TemplateErrors.Count > 0)
                    {
                        throw new BusinessException(message: "模板错误！当前模板中字段不匹配！！请正确上传模板数据！导入不对的列名：" + import.TemplateErrors.FirstOrDefault().RequireColumnName);
                    }
                    import.ShouldNotBeNull();
                    if (import.Exception != null)
                    {
                        //导入的数据有异常
                        throw new BusinessException(message:import.Exception.ToString());
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
                        //returnResult.errTemplate = Path.GetFileNameWithoutExtension(FileOriginName) + "_.xlsx";//返回错误模板名称作为参数
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
                throw new BusinessException(message: string.Join("\r\n", _errorList));
            }
            return ImportList;//返回客户端
        }

        /// <summary>
        /// 单个附件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public virtual async Task<List<T>> UploadExcelImport<T>([FromForm] IFormFile file)
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

            if (file == null)
            {
                throw new BusinessException(message:"上传附件不能为空！");
            }

            string FileOriginName = file.FileName;
            string getFileName = Path.GetFileName(FileOriginName);//获取附件名称
            using (var memoryStream = new MemoryStream())
            {
                //保存成物理文件
                await file.CopyToAsync(memoryStream);
                await _fileStorageBlobAppService.SaveBlobAsync(
                    new SaveFileBlobInputDto
                    {
                        Name = Path.GetFileName(FileOriginName),
                        Content = memoryStream.ToArray()
                    }
                );
            }
            //string fileSavePath = ConfigDirHelper.GetAppSetting("App", "FileRootPath");

            //文件保存的路径(应用的工作目录+文件夹相对路径);
            string fileSavePath = Environment.CurrentDirectory + @"\wwwroot\files\host\abp-file-container\";
            var filePath = fileSavePath + getFileName;//获取到导入的excel
            var import = await _importer.Import<T>(filePath);
            if (import.Exception != null)
            {
                if (import.Exception.Message.ToString() == "导入文件不存在!")
                {
                    throw new BusinessException(message:"文件容器配置的路径错误,请检查！");
                }
                else
                {
                    throw new BusinessException(message: import.Exception.Message.ToString());
                }
            }
            else
            {
                if (import.TemplateErrors.Count > 0)
                {
                    throw new BusinessException(message: "模板错误！当前模板中字段不匹配！！请正确上传模板数据！导入不对的列名：" + import.TemplateErrors.FirstOrDefault().RequireColumnName);
                }
                import.ShouldNotBeNull();
                if (import.Exception != null)
                {
                    //导入的数据有异常
                    throw new BusinessException(message: import.Exception.ToString());
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

            if (_errorList.Count > 0)
            {
                throw new BusinessException(message: string.Join("\r\n", _errorList));
            }
            return ImportList;//返回客户端
        }

        /// <summary>
        /// 输出报错信息（生成excel格式的文件）
        /// </summary>
        /// <param name="errorList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected async Task<string> ExportErrorReportAsync(List<ErrorExportDto> errorList, string fileName = "")
        {
            //没有信息返回成功
            if (errorList == null || errorList.Count == 0)
            {
                return ApplicationConsts.SuccessStr;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                //导出文件名称
                fileName = ExcelCommonMethod.GetExcelFileNameByUserID(ApplicationConsts.CheckErroFileName, "", ApplicationConsts.FileExtension);
            }

            errorList = errorList.Distinct().OrderBy(p => p.Type).ThenBy(p => p.Model).ThenBy(p => p.ItemCode).ToList();


            var result = await _exporter.ExportAsByteArray(errorList);

            result.ShouldNotBeNull();

            //保存导出文件到服务器存成二进制
            await _fileStorageBlobAppService.SaveBlobAsync(
                     new SaveFileBlobInputDto
                     {
                         Name = fileName,
                         Content = result
                     }
                 );
            return fileName;
        }



        //public Task<string>


        /// <summary>
        /// 生成导出的文件
        /// </summary>
        /// <param name="errorList"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> ExportFileAsync<T>(List<T> errorList, string fileNameTitle, string extension = ApplicationConsts.FileExtension) where T : class, new()
        {
            //未命名给赋值默认值
            if (string.IsNullOrEmpty(fileNameTitle))
            {
                fileNameTitle = ApplicationConsts.DefaultExportFileName;
            }


            //导出文件名称
            string fileName = ExcelCommonMethod.GetExcelFileNameByUserID(fileNameTitle, "", extension);

            var result = await _exporter.ExportAsByteArray(errorList);

            result.ShouldNotBeNull();

            //保存导出文件到服务器存成二进制
            await _fileStorageBlobAppService.SaveBlobAsync(
                     new SaveFileBlobInputDto
                     {
                         Name = fileName,
                         Content = result
                     }
                 );
            return fileName;
        }

    }
}
