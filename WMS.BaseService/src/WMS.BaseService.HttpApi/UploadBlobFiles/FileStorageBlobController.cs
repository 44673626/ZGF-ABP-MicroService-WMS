using WMS.BaseService.Settings;
using WMS.BaseService.CommonManagement.UploadBlobFiles;
using WMS.BaseService.CommonManagement.UploadBlobFiles.Dto;
using WMS.BaseService.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace WMS.BaseService.UploadBlobFiles
{
    /// <summary>
    /// 基础Blob二进制文件存储
    /// </summary>
    [Area(WMSBaseRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = WMSBaseRemoteServiceConsts.RemoteServiceName)]
    [Route($"{ApiConsts.RootPath}fileblob")]
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class FileStorageBlobController : WMSBaseController, IFileStorageBlobAppService
    {
        private readonly IFileStorageBlobAppService _fileContainer;//文件类型的Blob容器
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="fileContainer"></param>
        public FileStorageBlobController(IFileStorageBlobAppService fileContainer)
        {
            _fileContainer = fileContainer;
        }
        /// <summary>
        /// 保存二进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SaveBlobFile")]
        public virtual async Task SaveBlobAsync(SaveFileBlobInputDto input)
        {
            await _fileContainer.SaveBlobAsync(input);
        }
        /// <summary>
        /// 读取二进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBlobFile")]
        public virtual async Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input)
        {
            return await _fileContainer.GetBlobAsync(input);

        }
    }
}
