using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.FileManagement.Dto;
using FileStorage.UploadFile.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Win.Sfs.FileStorage.UploadFile;
using Win.Sfs.FileStorage.UploadFile.Dto;

namespace Win.Sfs.FileStorage
{
    [Route("api/getBlobFiles")]
    public class BlobFileController : FileStorageController, IFileStorageCommonService, IFileStorageBlobAppService
    {
        private readonly IFileStorageBlobAppService _fileAppService;
        private readonly IFileStorageBlobImgAppService _fileImgAppService;
        private readonly IFileStorageCommonService _upoladfileAppService;

        public BlobFileController() { }

        public BlobFileController(IFileStorageCommonService uploadfileAppService,
         IFileStorageBlobImgAppService fileImgAppService,
         IFileStorageBlobAppService fileAppService)
        {
            _upoladfileAppService = uploadfileAppService;
            _fileAppService = fileAppService;
            _fileImgAppService = fileImgAppService;
        }

        /// <summary>
        /// 上传单个文件并返回上传文件名
        /// </summary>
        /// <param name="file"></param>
        /// <param name="basedataId"></param>
        /// <param name="basedataName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFileName")]
        public virtual async Task<string> UploadFilesPost([FromForm] IFormFile file, [FromForm] string basedataId, [FromForm] string basedataName)
        {
            return await _upoladfileAppService.UploadFilesPost(file, basedataId, basedataName);
        }

        /// <summary>
        /// 获取Blob文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBlobFile")]
        public virtual async Task<BlobFilesDto> GetBlobAsync(GetBlobFileRequestDto input)
        {
            return await _fileAppService.GetBlobAsync(input);
        }

        /// <summary>
        /// 保存Blob文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveBlobFile")]
        public virtual async Task SaveBlobAsync(SaveFileBlobInputDto input)
        {
            await _fileAppService.SaveBlobAsync(input);
        }

        /// <summary>
        /// 单附件上传（统一用批量上传）
        /// </summary>
        /// <param name="File">上传文件</param>
        /// <param name="basedataId">所属模块ID</param>
        /// <param name="basedataName">所属模块的名称</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFilePost")]
        public virtual async Task<UploadFileInfoDto> UploadFilePost([FromForm] IFormFile File, [FromForm] string basedataId, [FromForm] string basedataName)
        {
            return await _upoladfileAppService.UploadFilePost(File, basedataId, basedataName);
        }

        /// <summary>
        /// 带过滤条件(附件名称，支持模糊查询)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getAllFileByFilter")]
        public virtual async Task<PagedResultDto<UploadFileInfoDto>> GetAll(GetUploadFiletInputDto input)
        {
            return await _upoladfileAppService.GetAll(input);
        }

        /// <summary>
        /// 根据模块ID获取附件信息（一对多关系）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getFilesByBaseDataId")]
        public virtual async Task<PagedResultDto<UploadFileInfoDto>> GetAllOfBaseDataID(GetFilesOfDataIDInputDto input)
        {
            return await _upoladfileAppService.GetAllOfBaseDataID(input);
        }

        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteFile")]
        public virtual async Task DeleteFile(List<Guid> ids)
        {
            await _upoladfileAppService.DeleteFile(ids);
        }

        /// <summary>
        /// 批量接收附件/IFormFileCollection
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFilesPost")]
        public virtual async Task<UploadFileInfoDto> UploadFilesPost([FromForm] IFormFileCollection files, [FromForm] string basedataId, [FromForm] string basedataName)
        {
            return await _upoladfileAppService.UploadFilesPost(files, basedataId, basedataName);
        }

        /// <summary>
        /// 传入流文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="basedataId"></param>
        /// <param name="basedataName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFilesBytePost")]
        public virtual async Task<UploadFileInfoDto> UploadFilesPost(byte[] bytes, string basedataId, string basedataName)
        {
            return await _upoladfileAppService.UploadFilesPost(bytes, basedataId, basedataName);
        }
        /// <summary>
        /// 根据ID查找附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public virtual async Task<UploadFileInfoDto> GetFile(Guid id)
        {
            return await _upoladfileAppService.GetFile(id);
        }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("download/{fileName}")]
        public virtual async Task<IActionResult> DownloadAsync(string fileName)
        {
            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobFileRequestDto { Name = fileName });

            return File(fileDto.Content, "application/octet-stream", fileDto.Name);
        }



        /// <summary>
        /// 获取图片类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("downloadImg/{fileName}")]
        public virtual async Task<IActionResult> DownloadImgAsync(string fileName)
        {
            var fileDto = await _fileImgAppService.GetBlobAsync(new GetBlobImgsRequestDto { Name = fileName });

            return File(fileDto.Content, "application/octet-stream", fileDto.Name);
        }
    }
}