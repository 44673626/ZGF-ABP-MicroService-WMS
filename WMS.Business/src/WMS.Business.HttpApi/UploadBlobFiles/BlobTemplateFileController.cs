using WMS.Business.Settings;
using WMS.Business.CommonManagement.UploadBlobFiles;
using WMS.Business.CommonManagement.UploadBlobFiles.comm;
using WMS.Business.CommonManagement.UploadBlobFiles.Dto;
using WMS.Business.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace WMS.Business.UploadBlobFiles
{
    /// <summary>
    /// 模板管理
    /// </summary>
    [Area(ABPVNextRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = ABPVNextRemoteServiceConsts.RemoteServiceName)]
    [Route($"{ApiConsts.RootPath}getBlobTemplateFiles")]
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class BlobTemplateFileController : ABPVNextController, IFileTemplateBlobAppService
    {
        private readonly IFileTemplateBlobAppService _fileAppService;

        public BlobTemplateFileController() { }

        public BlobTemplateFileController(IFileTemplateBlobAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        /// <summary>
        /// 获取Blob文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBlobFile")]
        public virtual async Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input)
        {
            return await _fileAppService.GetBlobAsync(input);
        }

        /// <summary>
        /// 保存Blob文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBlobFile")]
        public virtual async Task SaveBlobAsync(SaveFileBlobInputDto input)
        {
            await _fileAppService.SaveBlobAsync(input);
        }


        /// <summary>
        /// 上传模板到指定容器
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFilesPost")]
        public virtual async Task<UploadFileInfoDto> UploadFilesPost(IFormFileCollection files)
        {
            string fileSaveRootDir = ConfigDirHelper.GetAppSetting("App", "FileRootPath");
            string fileSaveDir = ConfigDirHelper.GetAppSetting("App", "WMSFiles");
            string absoluteFileDir = fileSaveRootDir + @"\" + fileSaveDir;
            string filepath = Environment.CurrentDirectory + @"\wwwroot\template" + absoluteFileDir;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //生成文件的名称
                    string Extension = Path.GetExtension(file.FileName);
                    if (string.IsNullOrEmpty(Extension))
                    {
                        throw new UserFriendlyException("文件上传的原始名称有误，没有找到文件后缀");
                    }
                    string fileName = file.FileName;
                    var currentpath = filepath + fileName;
                    if (System.IO.File.Exists(currentpath) == true)
                    {
                        continue;
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        currentpath = filepath + @"\" + fileName;
                        await _fileAppService.SaveBlobAsync(
                               new SaveFileBlobInputDto
                               {
                                   Name = fileName,
                                   Content = memoryStream.ToArray()
                               }
                           );
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 模板下载功能
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("download/{fileName}")]
        public virtual async Task<IActionResult> DownloadAsync(string fileName)
        {
            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobFileRequestDto { Name = fileName });

            return File(fileDto.Content, "application/octet-stream", fileDto.Name);
        }

    }
}
