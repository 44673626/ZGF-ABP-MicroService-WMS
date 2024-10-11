using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Win.Sfs.FileStorage.UploadFile.Dto;

namespace Win.Sfs.FileStorage.UploadFile
{
    /// <summary>
    /// 文件类型的Blob存储接口
    /// </summary>
    public interface IFileStorageBlobAppService : IApplicationService
    {
        Task SaveBlobAsync(SaveFileBlobInputDto input);

        Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input);
    }
}
