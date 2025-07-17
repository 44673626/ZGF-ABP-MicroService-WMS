using WMS.BaseService.CommonManagement.Tenants.Dto;
using WMS.BaseService.Dappers;
using WMS.BaseService.EntityFrameworkCore;
using WMS.BaseService.Samples;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Thinktecture;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Volo.Abp.TenantManagement.TenantManagementPermissions;

namespace WMS.BaseService.CommonManagement.Tenants
{
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class TenantManagerAppService : ApplicationService, ITenantManagerAppService
    {
        //集成Dapper
        private readonly DapperDbContext _dbDapperContext;
        //获取租户的仓储
        private readonly ITenantRepository _tenantRepository;
        //获取当前租户
        private readonly ICurrentTenant _currentTenant;
        //用于读取配置文件
        private readonly IConfiguration _configuration;
        //以下为执行种子数据定义的仓储，有新增可持续增加
        private readonly IRepository<Bom, Guid> _companyRepository;
        //注册dapper
        private readonly IDapperRepository _dapperRepository;

        public TenantManagerAppService(
            IRepository<Bom, Guid> companyRepository,
            ICurrentTenant currentTenant,
            DapperDbContext dbDapperContext,
            IDapperRepository dapperRepository,
            ITenantRepository tenantRepository,
           IConfiguration configuration)
        {
            _currentTenant = currentTenant;
            _companyRepository = companyRepository;
            _configuration = configuration;
            _tenantRepository = tenantRepository;
            _dbDapperContext = dbDapperContext;
            _dapperRepository = dapperRepository;
        }

        /// <summary>
        /// 创建宿主的种子数据及所有租户的种子数据初始化
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public virtual async Task CreateTenantDataBaseWithSeedData(bool isTenantConnExist = false)
        {
            List<string> errStrs = new List<string>();
            errStrs.Clear();
            //针对所有租户数据库及其种子数据的操作
            var tenants = await _tenantRepository.GetListAsync(includeDetails: true);
            if (tenants == null || tenants.Count <= 0)
            {
                throw new BusinessException(message: "租户信息不能为空，请先维护租户信息！");
            }
            #region 强制检验每个租户的独立数据库连接是否存在
            if (isTenantConnExist == true)
            {
                foreach (var tenant in tenants)
                {
                    if (tenant.ConnectionStrings.Count <= 0)
                    {
                        var currentTenant = await _tenantRepository.FindAsync(tenant.Id);//当前【租户】信息
                        string errinfo = $" 租户：{currentTenant.Name},未设置独立的数据库连接，请先维护！";
                        errStrs.Add(errinfo);
                    }
                }
                if (errStrs.Count > 0)
                {
                    string errstr = string.Empty;
                    foreach (var err in errStrs)
                    {
                        errstr += err + ",";
                    }
                    throw new BusinessException(message: errstr.TrimEnd(',').ToString());
                }
            }
            #endregion
            await GenerateSeedDataForTenant(null);//针对宿主种子数据的操作
            if (tenants.Any())
            {
                foreach (var tenant in tenants)
                {
                    if (tenant.ConnectionStrings.Any())
                    {
                        var tenantID = tenant.ConnectionStrings
                              .FirstOrDefault((x) => x.TenantId == tenant.Id);
                        if (tenantID != null)//判断，取的是ABP库中，该租户是否有独立数据库
                        {
                            //取ABP表（AbpTenantConnectionStrings）中每个租户对应的数据库连接字符串
                            var tenantConnectionString = tenant.ConnectionStrings
                            .Select(x => x.Value)
                            .FirstOrDefault();
                            #region 改变每个业务仓储的ConnectionString数据库连接字符串
                            var dbConnectionBom = (await _companyRepository.GetDbContextAsync()).Database.GetDbConnection();
                            dbConnectionBom.ConnectionString = tenantConnectionString;
                            #endregion
                            await GenerateSeedDataForTenant(tenant.Id);//有独立数据库后才可以迁移租户种子数据
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建租户数据库表结构（不包含种子数据）
        /// </summary>
        /// <param name="isTenantConnExist"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public virtual async Task CreateTenantDataBase(bool isTenantConnExist = false)
        {
            List<string> errStrs = new List<string>();
            errStrs.Clear();
            //针对所有租户数据库及其种子数据的操作
            var tenants = await _tenantRepository.GetListAsync(includeDetails: true);
            if (tenants == null || tenants.Count <= 0)
            {
                throw new BusinessException(message: "租户信息不能为空，请先维护租户信息！");
            }

            #region 强制检验每个租户的独立数据库连接是否存在
            if (isTenantConnExist == true)
            {
                foreach (var tenant in tenants)
                {
                    if (tenant.ConnectionStrings.Count <= 0)
                    {
                        var currentTenant = await _tenantRepository.FindAsync(tenant.Id);//当前【租户】信息
                        string errinfo = $" 租户：{currentTenant.Name},未设置独立的数据库连接，请先维护！";
                        errStrs.Add(errinfo);
                    }
                }
                if (errStrs.Count > 0)
                {
                    string errstr = string.Empty;
                    foreach (var err in errStrs)
                    {
                        errstr += err + ",";
                    }
                    throw new BusinessException(message: errstr.TrimEnd(',').ToString());
                }
            }
            #endregion

            if (tenants.Any())
            {
                foreach (var tenant in tenants)
                {
                    if (tenant.ConnectionStrings.Any())
                    {
                        var tenantID = tenant.ConnectionStrings
                              .FirstOrDefault((x) => x.TenantId == tenant.Id);
                        if (tenantID != null)//判断，取的是ABP库中，该租户是否有独立数据库
                        {
                            //取ABP表（AbpTenantConnectionStrings）中每个租户对应的数据库连接字符串
                            var tenantConnectionString = tenant.ConnectionStrings
                            .Select(x => x.Value)
                            .FirstOrDefault();
                            //配置当前租户的数据库
                            var dbContextOptions = new DbContextOptionsBuilder<WMSBaseDbContext>()
                                  .UseSqlServer(tenantConnectionString, builder => builder.AddTableHintSupport())
                                  .Options;
                            using (var tenantDbContext = new WMSBaseDbContext(dbContextOptions))
                            {
                                try
                                {
                                    // 如果数据库表不存在则创建
                                    tenantDbContext.Database.EnsureCreated();
                                    // 执行迁移操作
                                    await tenantDbContext.Database.MigrateAsync();
                                    // 其他针对每个租户的操作...                                                                                           
                                    tenantDbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    throw new BusinessException(message: ex.Message);// 处理迁移过程中的错误...
                                }
                            }

                        }
                    }
                }
            }
        }
        /// <summary>
        /// 根据租户的 ID 生成相应的初始种子数据
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <returns></returns>
        private async Task GenerateSeedDataForTenant(Guid? tenantId)
        {
            //租户的变更，需要调用Change方法（自动切租户的独立数据库）
            using (_currentTenant.Change(tenantId))
            {
                #region 先删除
                if (await _companyRepository.GetCountAsync() > 0)
                {
                    await _companyRepository.DeleteAsync(x => true);
                }
                #endregion

                #region 迁移的种子数据
                //Bom业务为例子
                Bom company = new Bom(Guid.NewGuid(), "adbcc", "1", "2", "3", "4", 5, 6, 7, "8", 
                    DateTime.Now, DateTime.Now.AddDays(1), "9", 10, Guid.NewGuid(), "11");
                await _companyRepository.InsertAsync(company, true);
                #endregion
            }
        }

        /// <summary>
        /// 上传执行SQL脚本,实现所有租户数据库表结构的更改或新增
        /// </summary>
        /// <param name="files">上传的SQL脚本</param>
        /// <param name="tenantId">租户ID</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task TenantUploadSqlScriptImport([FromForm] IFormFileCollection files, Guid? tenantId)
        {
            foreach (var file in files)
            {

                #region 租户的检验
                List<string> errStrs = new List<string>();
                errStrs.Clear();

                //针对所有租户数据库及其种子数据的操作
                var tenants = await _tenantRepository.GetListAsync(includeDetails: true);
                if (tenants == null || tenants.Count <= 0) throw new BusinessException("租户信息不能为空，请先维护租户信息！");
                //检验，每个租户的独立数据库连接是否存在
                foreach (var tenant in tenants)
                {
                    if (tenant.ConnectionStrings.Count <= 0)
                    {
                        var currentTenant = await _tenantRepository.FindAsync(tenant.Id);//当前【租户】信息
                        string errinfo = $" 租户：{currentTenant.Name},未设置独立的数据库连接，请先维护！";
                        errStrs.Add(errinfo);
                    }
                }
                if (errStrs.Count > 0)
                {
                    string errstr = string.Empty;
                    foreach (var err in errStrs)
                    {
                        errstr += err + ",";
                    }
                    throw new BusinessException("01", errstr.TrimEnd(',').ToString());
                }
                #endregion


                #region 相关上传验证和文件存储
                if (file == null || file.Length == 0) throw new BusinessException("上传附件不能为空");
                //限制100M
                if (file.Length > 104857600)
                {
                    throw new BusinessException("上传文件过大!");
                }
                string fileExt = Path.GetExtension(file.FileName);
                if (string.IsNullOrEmpty(fileExt))
                {
                    throw new UserFriendlyException("文件上传的原始名称有误，没有找到文件后缀");
                }
                if (fileExt.ToUpper() != ".SQL")
                {
                    throw new UserFriendlyException("上传文件必须是SQL脚本文件！");
                }
                //存储到指定文件路径，存储到项目wwwroot目录下
                string uploadsFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot", "files");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = Guid.NewGuid().ToString() + fileExt;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);//保存
                    fileStream.Flush();
                }
                #endregion

                //读取SQL脚本文件
                var scriptSql = File.ReadAllText(filePath);
                //指定的格式如下
                //string sql =
                //    @"ALTER TABLE [MDS_Material] ADD [Extand] nvarchar(max) NOT NULL DEFAULT N'';
                //      GO
                //    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230713065604_20230713-v2', N'7.0.1');
                //     GO                    ";

                foreach (var tenant in tenants)
                {
                    if (tenant.ConnectionStrings.Any())
                    {
                        var tenantID = tenant.ConnectionStrings
                              .FirstOrDefault((x) => x.TenantId == tenant.Id);
                        if (tenantID != null)//判断，取的是ABP库中，该租户是否有独立数据库
                        {
                            //取ABP表（AbpTenantConnectionStrings）中每个租户对应的数据库连接字符串
                            var tenantConnectionString = tenant.ConnectionStrings
                            .Select(x => x.Value)
                            .FirstOrDefault();
                            //配置当前租户的数据库
                            var dbContextOptions = new DbContextOptionsBuilder<WMSBaseDbContext>()
                                  .UseSqlServer(tenantConnectionString)
                                  .Options;
                            using (var tenantDbContext = new WMSBaseDbContext(dbContextOptions))
                            {
                                try
                                {
                                    //格式化
                                    var sqlArr = Regex.Split(scriptSql.Trim(), "\r\n\\s*go", RegexOptions.IgnoreCase);
                                    foreach (string strsql in sqlArr)
                                    {
                                        if (strsql.Trim().Length > 1 && strsql.Trim() != "\r\n")
                                        {
                                            // 执行SQL脚本
                                            await tenantDbContext.Database.ExecuteSqlRawAsync(strsql);
                                        }
                                    }
                                    // 其他针对每个租户的操作...                                                                                           
                                    tenantDbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    // 处理异常错误...
                                }
                            }

                        }
                    }
                }

            }

        }

        /// <summary>
        /// 创建租户的独立数据库连接(使用Dapper)
        /// </summary>
        /// <param name="dto">添加的表信息</param>
        /// <returns></returns>
        public async Task<int> CreateAbpTenantConnectionStrings(TenantConnectionStringsDto dto)
        {
            //针对所有租户数据库及其种子数据的操作
            var tenants = await _tenantRepository.GetListAsync(includeDetails: true);
            if (tenants == null || tenants.Count <= 0)
            {
                throw new BusinessException("租户信息不能为空，请先维护租户信息！");
            }
            else
            {
                var currentTenant = await _tenantRepository.FindAsync(dto.TenantId.Value);//当前【租户】信息
                if (currentTenant == null)
                {
                    throw new BusinessException(message: "在租户表中未找到该租户信息！");
                }
            }
            string sql = @$"INSERT INTO [AbpTenantConnectionStrings]
                ([TenantId],[Name],[Value])VALUES('{dto.TenantId}','{dto.Name}','{dto.TenantConnection}') ";
            var dbConnection = await _dapperRepository.GetDbConnectionAsync();
            return await dbConnection.ExecuteAsync(sql);//使用dapper
            //return await _dbDapperContext.ExecuteAsync(sql, databaseType: DatabaseType.Default);
        }





    }
}
