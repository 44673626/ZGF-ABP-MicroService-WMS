using WMS.Business.CommonManagement.UploadBlobFiles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WMS.Business.CommonManagement.UploadBlobFiles
{
    /// <summary>
    /// 用于存储模板文件
    /// </summary>
    public interface IFileTemplateBlobAppService : IApplicationService
    {
        Task SaveBlobAsync(SaveFileBlobInputDto input);

        Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input);
    }
}
