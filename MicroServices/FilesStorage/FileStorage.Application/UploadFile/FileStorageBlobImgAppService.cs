using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Win.Sfs.FileStorage.UploadFile.Dto;

namespace Win.Sfs.FileStorage.UploadFile
{
    /// <summary>
    /// 图片类型的Blob存储服务
    /// </summary>
    public class FileStorageBlobImgAppService : ApplicationService, IFileStorageBlobImgAppService
    {
        private readonly IBlobContainer<BlobImgFileContainer> _fileImgContainer;//图片类型文件的Blob容器

        public FileStorageBlobImgAppService(IBlobContainer<BlobImgFileContainer> fileImgContainer)
        {
            _fileImgContainer = fileImgContainer;
        }
        /// <summary>
        /// 保存blob
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task SaveBlobAsync(SaveFileBlobImgInputDto input)
        {
            await _fileImgContainer.SaveAsync(input.Name, input.Content, true);
        }
        /// <summary>
        /// 获取BLOB存储
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<BlobImgsDto> GetBlobAsync(GetBlobImgsRequestDto input)
        {
            var blob = await _fileImgContainer.GetAllBytesAsync(input.Name);
            return new BlobImgsDto
            {
                Name = input.Name,
                Content = blob
            };

        }
    }
}
