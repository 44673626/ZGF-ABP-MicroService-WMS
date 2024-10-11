using WMS.Business.CommonManagement.UploadBlobFiles.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace WMS.Business.CommonManagement.UploadBlobFiles
{
    /// <summary>
    /// 用于存储模板
    /// </summary>
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class FileTemplateBlobAppService : ApplicationService, IFileTemplateBlobAppService
    {
        private readonly IBlobContainer<BlobTemplateFileContainer> _fileContainer;//文件类型的Blob容器

        public FileTemplateBlobAppService(IBlobContainer<BlobTemplateFileContainer> fileContainer)
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
