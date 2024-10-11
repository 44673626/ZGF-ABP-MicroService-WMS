using ABP.Business.CommonManagement.UploadBlobFiles.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ABP.Business.CommonManagement.UploadBlobFiles
{
    public interface IFileStorageCommonService : IApplicationService
    {

        /// <summary>
        /// 上传单个文件并返回文件名
        /// </summary>
        /// <param name="file"></param>
        /// <param name="basedataId"></param>
        /// <param name="basedataName"></param>
        /// <returns></returns>
        Task<string> UploadFilesPostBySigle(IFormFile file, string basedataId, string basedataName);

        /// <summary>
        /// 单个附件上传 
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        Task<UploadFileInfoDto> UploadFilePostSigle([FromForm] IFormFile File, [FromForm] string basedataId, [FromForm] string basedataName);

        /// <summary>
        /// 批量上传
        /// </summary>
        /// <param name="files"></param>
        /// <param name="basedataId">模块ID</param>
        /// <param name="basedataName">模块名称</param>
        /// <returns></returns>
        Task<UploadFileInfoDto> UploadFilesPost([FromForm] IFormFileCollection files, [FromForm] string basedataId, [FromForm] string basedataName);

        /// <summary>
        /// 根据ID查找附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UploadFileInfoDto> GetFile(Guid id);

        /// <summary>
        /// 进行批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteFile(List<Guid> ids);

        Task<UploadFileInfoDto> UploadFilesPostByte(byte[] bytes, string basedataId, string basedataName);

        /// <summary>
        /// 获取所有附件列表,可带过滤条件Filte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UploadFileInfoDto>> GetAll(GetUploadFiletInputDto input);

    }
}
