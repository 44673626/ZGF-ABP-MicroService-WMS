using FileStorage.FileManagement.Dto;
using FileStorage.UploadFile.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Win.Sfs.FileStorage.UploadFile
{
    public interface IFileStorageCommonService : IApplicationService
    {
        /// <summary>
        /// 批量上传附件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        //Task<UploadFileInfoDto> UploadFilesPost([FromForm] IFormFileCollection files);

        //上传单个文件并返回文件名
        Task<string> UploadFilesPost(IFormFile file, string basedataId, string basedataName);

        /// <summary>
        /// 单个附件上传 
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        Task<UploadFileInfoDto> UploadFilePost([FromForm] IFormFile File, [FromForm] string basedataId, [FromForm] string basedataName);

       /// <summary>
       /// 批量上传
       /// </summary>
       /// <param name="files"></param>
       /// <param name="basedataId">模块ID</param>
       /// <param name="basedataName">模块名称</param>
       /// <returns></returns>
        Task<UploadFileInfoDto> UploadFilesPost([FromForm] IFormFileCollection files, [FromForm] string basedataId, [FromForm] string basedataName);

        /// <summary>
        /// 获取所有附件列表,可带过滤条件Filte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UploadFileInfoDto>> GetAll(GetUploadFiletInputDto input);

        /// <summary>
        /// 根据基础信息类别ID进行过滤-BaseDataID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<UploadFileInfoDto>> GetAllOfBaseDataID(GetFilesOfDataIDInputDto input);

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

        Task<UploadFileInfoDto> UploadFilesPost(byte[] bytes, string basedataId, string basedataName);

    }
}
