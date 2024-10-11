using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp;
using Volo.Abp.TenantManagement;
using static Volo.Abp.TenantManagement.TenantManagementPermissions;
using WMS.BaseService.BaseData;
using Microsoft.EntityFrameworkCore;
using WMS.BaseService.Settings;
using WMS.BaseService.CommonManagement.Tenants;
using WMS.BaseService.CommonManagement.Tenants.Dto;
using WMS.BaseService.Settings;

namespace WMS.BaseService.Tenants
{
    /// <summary>
    /// 根据租户的独立数据库，自动生成租户表及其种子数据
    /// </summary>
    [Area(ABPVNextRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = ABPVNextRemoteServiceConsts.RemoteServiceName)]
    [Route($"{ApiConsts.RootPath}TenantsManager")]
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class TenantsController : ABPVNextController
    {
        private readonly ITenantManagerAppService _iTenantManager;
        //读取配置文件
        private readonly IConfiguration _configuration;

   
        public TenantsController(
            ITenantManagerAppService iTenantManager,
           IConfiguration configuration)
        {
            _configuration = configuration;
            _iTenantManager= iTenantManager;
        }

        /// <summary>
        ///  根据宿主的数据库同步创建租户数据库表结构（不包含初始的种子数据）
        /// </summary>
        /// <param name="isTenantConnExist"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("createTenatDataBase")]
        [UnitOfWork]
        public virtual async Task CreateTenantDataBase(bool isTenantConnExist = false)
        {
            await _iTenantManager.CreateTenantDataBase(isTenantConnExist);
        }

        /// <summary>
        /// 创建宿主的种子数据及所有租户的种子数据初始化（辅助功能）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("createTenatWithSeedData")]
        [UnitOfWork]
        public virtual async Task CreateTenantDataBaseWithSeedData(bool isTenantConnExist = false)
        {
            await _iTenantManager.CreateTenantDataBaseWithSeedData(isTenantConnExist);
        }

      

        /// <summary>
        /// 上传执行SQL脚本,实现所有租户数据库表结构的更改或新增
        /// </summary>
        /// <param name="files"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="UserFriendlyException"></exception>
        [HttpPost]
        [Route("tenant-SqlScript-Import")]
        [DisableRequestSizeLimit]
        public async Task TenantUploadSqlScriptImport([FromForm] IFormFileCollection files, Guid? tenantId)
        {
            await _iTenantManager.TenantUploadSqlScriptImport(files, tenantId);
        }

        /// <summary>
        /// 创建租户独立的数据库连接配置，操作TenantConnectionStrings表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("tenant-connection-strings")]
        public async Task<int> CreateAbpTenantConnectionStrings(TenantConnectionStringsDto dto)
        {
            return await _iTenantManager.CreateAbpTenantConnectionStrings(dto);
        }

        /// <summary>
        /// 大文件分片下载
        /// </summary>
        /// <param name="fileId">文件唯一标识</param>
        /// <param name="index">当前索引</param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("zip-download")]
        //[DisableRequestSizeLimit]
        //public async Task DownloadBigFile(string fileId, int index)
        //{
        //    HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");
        //    try
        //    {
        //        //存储到指定文件路径，存储到项目wwwroot目录下
        //        string uploadsFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot", "files");
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }
        //        var uniqueFileName = "ABP-MicroService-master7.0.rar";
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
        //        {
        //            HttpContext.Response.Headers.Add("FileResult", "0");
        //            HttpContext.Response.Headers.Add("FileMaxIndex", "0");
        //            HttpContext.Response.Headers.Add("FileIndex", index + "");
        //            throw new BusinessException(message: "下载失败:未找到文件！");
        //        }
        //        var fileName = Path.GetFileName(filePath);
        //        var extension = Path.GetExtension(filePath);
        //        new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out var contenttype);

        //        if (!System.IO.File.Exists(filePath))
        //        {
        //            HttpContext.Response.Headers.Add("FileResult", "0");
        //            HttpContext.Response.Headers.Add("FileMaxIndex", "0");
        //            HttpContext.Response.Headers.Add("FileIndex", index + "");
        //            throw new BusinessException(message: "文件不存在！");
        //        }
        //        int shardSize = 100 * 1024 * 1024;//100M
        //        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {

        //            int count = (int)(fs.Length / shardSize);
        //            long chunk = fs.Seek(index * shardSize, SeekOrigin.Begin);
        //            if (index == count)
        //            {
        //                //最后一片 = 总长 - (每次片段大小 * 已下载片段个数)
        //                shardSize = (int)(fs.Length - (shardSize * index));
        //            }
        //            byte[] datas = new byte[shardSize];

        //            int offset = fs.Read(datas, 0, datas.Length);
        //            HttpContext.Response.Headers.Add("FileId", fileId);
        //            HttpContext.Response.Headers.Add("FileResult", "1");
        //            HttpContext.Response.Headers.Add("FileMaxIndex", count + "");
        //            HttpContext.Response.Headers.Add("FileIndex", index + "");
        //            File(datas, contenttype, fileName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Response.Headers.Add("FileResult", "0");
        //        HttpContext.Response.Headers.Add("FileMaxIndex", "0");
        //        HttpContext.Response.Headers.Add("FileIndex", index + "");
        //        throw new BusinessException(message: "下载失败！");
        //    }
        //}


    }
}
