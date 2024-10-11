using WMS.BaseService.CommonManagement.UploadBlobFiles.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles
{
    /// <summary>
    /// Excel文件类型的Blob存储服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class FileStorageBlobAppService : ApplicationService, IFileStorageBlobAppService
    {
        private readonly IBlobContainer<BlobCommonFileContainer> _fileContainer;//文件类型的Blob容器

        public FileStorageBlobAppService(IBlobContainer<BlobCommonFileContainer> fileContainer)
        {
            _fileContainer = fileContainer;
        }
        /// <summary>
        /// 保存blob
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task SaveBlobAsync(SaveFileBlobInputDto input)
        {
            await _fileContainer.SaveAsync(input.Name, input.Content, true);
        }
        /// <summary>
        /// 获取BLOB存储
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input)
        {
            var blob = await _fileContainer.GetAllBytesAsync(input.Name);
            return new BlobFilesDto
            {
                Name = input.Name,
                Content = blob
            };

        }
    }
}
