using ABP.Business.CommonManagement.UploadBlobFiles.Dto;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ABP.Business.CommonManagement.UploadBlobFiles
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
