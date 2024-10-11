using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Win.Sfs.FileStorage.UploadFile;
using Win.Sfs.FileStorage.UploadFile.Dto;

namespace Win.Sfs.FileStorage.UploadController
{
    [Route("api/getBlobFiles/fileblob")]
    public class FileStorageBlobController: FileStorageController, IFileStorageBlobAppService
    {
        private readonly IFileStorageBlobAppService _fileContainer;//文件类型的Blob容器

        public FileStorageBlobController(IFileStorageBlobAppService fileContainer)
        {
            _fileContainer = fileContainer;
        }

        [HttpGet]
        [Route("SaveBlobFile")]
        public virtual async Task SaveBlobAsync(SaveFileBlobInputDto input)
        {
            await _fileContainer.SaveBlobAsync(input);
        }

        [HttpPost]
        [Route("GetBlobFile")]
        public virtual async Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input)
        {
            return await _fileContainer.GetBlobAsync(input);

        }
    }
}
