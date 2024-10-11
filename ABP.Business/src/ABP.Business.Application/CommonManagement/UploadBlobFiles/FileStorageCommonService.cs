using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using ABP.Business.CommonManagement.UploadBlobFiles.comm;
using ABP.Business.CommonManagement.UploadBlobFiles.Dto;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Aspects;
using Volo.Abp.Auditing;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace ABP.Business.CommonManagement.UploadBlobFiles
{
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class FileStorageCommonService : ApplicationService, IFileStorageCommonService
    {
        private readonly IRepository<UploadFileInfo, Guid> _uploadfileRepository;
        private readonly IFileStorageBlobAppService _fileBlobAppService;

        public FileStorageCommonService() { }

        //构造函数依赖注入
        public FileStorageCommonService(
            IRepository<UploadFileInfo, Guid> uploadfileRepository,
            IFileStorageBlobAppService fileBlobAppService
            )
        {
            _fileBlobAppService = fileBlobAppService;
            _uploadfileRepository = uploadfileRepository;
        }


        /// <summary>
        /// 单附件上传（统一用批量上传）
        /// </summary>
        /// <param name="File">上传文件</param>
        /// <param name="basedataId">所属模块ID</param>
        /// <param name="basedataName">所属模块的名称</param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("UploadFilePostSigle")]
        public virtual async Task<UploadFileInfoDto> UploadFilePostSigle([FromForm] IFormFile File, [FromForm] string basedataId, [FromForm] string basedataName)
        {
            try
            {
                if (File == null)
                {
                    throw new BusinessException("上传附件不能为空！");
                }
                if (File.Length > 2097152) //10MB = 1024 * 1024 *2
                {
                    throw new BusinessException("上传附件大小能超过2M！");
                }
                var guid = GuidGenerator.Create();
                //文件的原始名称
                string FileOriginName = File.FileName;
                //读取文件保存的根目录
                string fileSaveRootDir = ConfigDirHelper.GetAppSetting("App", "FileRootPath");
                //读取WMS文件保存的模块的根目录
                string fileSaveDir = ConfigDirHelper.GetAppSetting("App", "WMSFiles");
                //文件保存的相对文件夹(保存到wwwroot目录下)
                string absoluteFileDir = fileSaveRootDir + @"\" + fileSaveDir;
                //文件保存的路径(应用的工作目录+文件夹相对路径);
                string fileSavePath = Environment.CurrentDirectory + @"\wwwroot\files" + absoluteFileDir;
                if (!Directory.Exists(fileSavePath))
                {
                    Directory.CreateDirectory(fileSavePath);
                }
                //生成文件的名称
                string Extension = Path.GetExtension(FileOriginName);//获取文件的源后缀
                if (string.IsNullOrEmpty(Extension))
                {
                    throw new UserFriendlyException("文件上传的原始名称有误，没有找到文件后缀");
                }
                string fileName = guid + Extension;//通过Guid和原始后缀生成新的文件名
                //最终生成的文件的相对路径（xxx/xxx/xx.xx）
                string finalyFilePath = fileSavePath + fileName;
                //打开上传文件的输入流
                Stream stream = File.OpenReadStream();
                // 把 Stream 转换成 byte[]
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                //await _blobContainer.SaveAsync(guid.ToString(), stream);
                //创建输入流的reader
                //var fileType = stream.GetFileType();
                //创建附件对象
                var uploadfile = new UploadFileInfo(
                    guid,
                    CurrentTenant.Id.Value,
                    Path.GetFileNameWithoutExtension(FileOriginName.ToString()),
                     Extension.Substring(1),
                    "",
                    finalyFilePath,
                    FileType.File,
                    basedataId,
                    basedataName,
                    bytes.Length
                    );
                //开始保存拷贝文件
                FileStream targetFileStream = new FileStream(finalyFilePath, FileMode.OpenOrCreate);
                await stream.CopyToAsync(targetFileStream);
                //附件对象保存到数据库
                var result = await _uploadfileRepository.InsertAsync(uploadfile);
                return ObjectMapper.Map<UploadFileInfo, UploadFileInfoDto>(result);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("文件上传失败，原因" + ex.Message);
            }

        }
        /// <summary>
        /// 根据ID查找附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("{id}")]
        public virtual async Task<UploadFileInfoDto> GetFile(Guid id)
        {
            var result = await _uploadfileRepository.GetAsync(id);
            return ObjectMapper.Map<UploadFileInfo, UploadFileInfoDto>(result);
        }

        /// <summary>
        /// 带过滤条件(附件名称，支持模糊查询)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("getAllFileByFilter")]
        public virtual async Task<PagedResultDto<UploadFileInfoDto>> GetAll(GetUploadFiletInputDto input)
        {
            var _query = await _uploadfileRepository.GetQueryableAsync();
            var query = _query
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), _ => _.FileName.Contains(input.Filter));
            var items = await query.OrderBy(_ => _.FileName)
                     .Skip(input.SkipCount)
                     .Take(input.MaxResultCount)
                     .ToListAsync();
            var dots = ObjectMapper.Map<List<UploadFileInfo>, List<UploadFileInfo>>(items);
            var totalCount = await query.CountAsync();
            return new PagedResultDto<UploadFileInfoDto>(totalCount, (IReadOnlyList<UploadFileInfoDto>)dots);
        }

        /// <summary>
        /// 批量删除附件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("DeleteFile")]
        //[Authorize(abpvnext_masterPermissions.MyUploadFile.Delete)]
        public virtual async Task DeleteFile(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                var fileToDelete = await GetFile(id);
                if (fileToDelete != null)
                {
                    await _uploadfileRepository.DeleteAsync(id);
                    //删除物理文件
                    var path = fileToDelete.FileUrl;
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
        }
        /// <summary>
        /// 批量接收附件/IFormFileCollection
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public virtual async Task<UploadFileInfoDto> UploadFilesPost(IFormFileCollection files, string basedataId, string basedataName)
        {

            long size = files.Sum(f => f.Length);       //统计所有文件的大小
            //读取文件保存的根目录
            string fileSaveRootDir = ConfigDirHelper.GetAppSetting("App", "FileRootPath");
            //读取WMS文件保存的模块的根目录
            string fileSaveDir = ConfigDirHelper.GetAppSetting("App", "WMSFiles");
            //文件保存的相对文件夹(保存到wwwroot目录下)
            string absoluteFileDir = fileSaveRootDir + @"\" + fileSaveDir;
            //文件保存的路径(应用的工作目录+文件夹相对路径);
            string filepath = Environment.CurrentDirectory + @"\wwwroot\files" + absoluteFileDir;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            foreach (var file in files)     //上传选定的文件列表
            {
                if (file.Length > 0)        //文件大小 0 才上传
                {
                    var guid = GuidGenerator.Create();
                    //生成文件的名称
                    string Extension = Path.GetExtension(file.FileName);//获取文件的源后缀
                    if (string.IsNullOrEmpty(Extension))
                    {
                        throw new UserFriendlyException("文件上传的原始名称有误，没有找到文件后缀");
                    }
                    string fileName = guid + Extension;//通过Guid和原始后缀生成新的文件名
                    var currentpath = filepath + fileName;     //当前上传文件应存放的位置
                    if (System.IO.File.Exists(currentpath) == true)        //如果文件已经存在,跳过此文件的上传
                    {
                        //throw new BusinessException("文件已存在：" + thispath.ToString());
                        continue;
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);//直接存二进制
                        currentpath = filepath + @"host\win-file-container\" + fileName;
                        //文本文件--存储到blob中
                        await _fileBlobAppService.SaveBlobAsync(
                               new SaveFileBlobInputDto
                               {
                                   Name = fileName,
                                   Content = memoryStream.ToArray()
                               }
                           );
                        //创建附件对象
                        var uploadfile = new UploadFileInfo(
                            guid,
                            CurrentTenant.Id,
                            Path.GetFileNameWithoutExtension(file.FileName.ToString()),
                             Extension.Substring(1),
                            "",
                            currentpath,
                            FileType.File,
                            basedataId,
                            basedataName,
                            memoryStream.Length
                            );
                        var result = await _uploadfileRepository.InsertAsync(uploadfile);//存入数据库
                        ObjectMapper.Map<UploadFileInfo, UploadFileInfoDto>(result);//映射

                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 上传单个文件并返回文件名
        /// </summary>
        /// <param name="file"></param>
        /// <param name="basedataId"></param>
        /// <param name="basedataName"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        /// <exception cref="BusinessException"></exception>
        public virtual async Task<string> UploadFilesPostBySigle(IFormFile file, string basedataId, string basedataName)
        {
            string fileName = "";
            //读取文件保存的根目录
            string fileSaveRootDir = ConfigDirHelper.GetAppSetting("App", "FileRootPath");
            //读取WMS文件保存的模块的根目录
            string fileSaveDir = ConfigDirHelper.GetAppSetting("App", "WMSFiles");
            //文件保存的相对文件夹(保存到wwwroot目录下)
            string absoluteFileDir = fileSaveRootDir + @"\" + fileSaveDir;
            //文件保存的路径(应用的工作目录+文件夹相对路径);
            string filepath = Environment.CurrentDirectory + @"\wwwroot\files" + absoluteFileDir;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            if (file.Length > 0)        //文件大小 0 才上传
            {
                var guid = GuidGenerator.Create();
                //生成文件的名称
                string Extension = Path.GetExtension(file.FileName);//获取文件的源后缀
                if (string.IsNullOrEmpty(Extension))
                {
                    throw new UserFriendlyException("文件上传的原始名称有误，没有找到文件后缀");
                }
                fileName = guid + Extension;//通过Guid和原始后缀生成新的文件名
                var currentpath = filepath + fileName;     //当前上传文件应存放的位置
                if (System.IO.File.Exists(currentpath) == true)        //如果文件已经存在,跳过此文件的上传
                {
                    throw new BusinessException("文件已存在(文件名不能相同)：" + currentpath.ToString());
                }
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);//直接存二进制

                    currentpath = filepath + @"host\win-file-container\" + fileName;
                    //文本文件--存储到blob中
                    await _fileBlobAppService.SaveBlobAsync(
                           new SaveFileBlobInputDto
                           {
                               Name = fileName,
                               Content = memoryStream.ToArray()
                           }
                       );
                    //创建附件对象
                    var uploadfile = new UploadFileInfo(
                        guid,
                        CurrentTenant.Id,
                        Path.GetFileNameWithoutExtension(file.FileName.ToString()),
                         Extension.Substring(1),
                        "",
                        currentpath,
                        FileType.File,
                        basedataId,
                        basedataName,
                        memoryStream.Length
                        );
                    var result = await _uploadfileRepository.InsertAsync(uploadfile);//存入数据库
                    ObjectMapper.Map<UploadFileInfo, UploadFileInfoDto>(result);//映射

                }
            }

            return fileName;
        }


        public virtual Task<UploadFileInfoDto> UploadFilesPostByte(byte[] bytes, string basedataId, string basedataName)
        {
            return null;
        }

    }
}
