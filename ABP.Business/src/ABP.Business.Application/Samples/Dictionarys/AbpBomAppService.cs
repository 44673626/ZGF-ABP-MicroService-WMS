using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Dapper;
using EFCore.BulkExtensions;
using ABP.Business.CommonManagement.UploadBlobFiles;
using Medallion.Threading;
using ABP.Business.Samples.DataDictionarys;
using ABP.Business.CommonManagement.UploadBlobFiles.comm;
using ABP.Business.Samples.Boms.Dto;
using ABP.Business.CommonManagement.UploadBlobFiles.ExportReports;
using ABP.Business.Settings;
using ABP.Business.Samples.Boms;
using Microsoft.AspNetCore.Authorization;
using ABP.Business.Settings;

namespace ABP.Business.Samples.Dictionarys
{
    /// <summary>
    /// 文件导入及导出功能
    /// </summary>
    [Route($"{ApiConsts.RootPath}ImportBom")]
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class AbpBomAppService : BaseDataExcelApplicationBase, IBomAppService
    {
        //仓储
        private readonly IRepository<Bom, Guid> _repository;
        private readonly IFileStorageBlobAppService _fileStorageBlobAppService;
        // public IFileStorageBlobAppService FileStorageBlobAppService { get; set; }



        public AbpBomAppService(
            IRepository<Bom, Guid> repository, IFileStorageBlobAppService fileStorageBlobAppService) : base(fileStorageBlobAppService)
        {
            _repository = repository;
            _fileStorageBlobAppService = fileStorageBlobAppService;
        }

        ////#region 导入导出功能
        /// <summary>
        /// 文件导入功能
        /// </summary>
        /// <param name="files"></param>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="version"></param>
        /// <param name="customerCode"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportBom")]
        public async Task<string> BomUploadExcelImport([FromForm] IFormFileCollection files)
        //[FromForm] string year, [FromForm] string period, [FromForm] string version, [FromForm] string customerCode, [FromForm] string factory)
        {
            #region 调用文件导入的通用方法
            var result = await UploadExcelImport<ImportBomDto>(files);
            var _id = GuidGenerator.Create();
            var entityList = ObjectMapper.Map<List<ImportBomDto>, List<Bom>>(result);
            #endregion

            #region 导入前先删除操作,后执行批量添加操作
            var query = await _repository.GetQueryableAsync();
            if (query != null && query.Count() > 0)
            {
                //var _query = query.Where(p => p.Version == version && p.Factory == factory);
                //await _query.BatchDeleteAsync();//批量删除
            }
            #endregion

            #region 导入前检验，将以excel方式输出检验信息
            var checkList = new List<ErrorExportDto>();
            var _group = entityList.GroupBy(x => new { x.ParentItemCode, x.ChildItemCode, x.Version }).Select(p => new { Count = p.Count(), ParentItmeCode = p.Key.ParentItemCode, p.Key.ChildItemCode });
            foreach (var itm in _group)
            {
                if (itm.Count > 1)
                {
                    checkList.Add(new ErrorExportDto("", "customerCode", string.Empty, string.Empty, string.Empty, string.Empty, string.Format("不能导入父编码{0},子编码{1}有重复数据", itm.ParentItmeCode, itm.ChildItemCode), string.Empty));
                }
            }

            if (checkList.Count > 0)
            {
                return await ExportErrorReportAsync(checkList);
            }
            #endregion

            #region 补充赋值
            foreach (var itm in entityList)
            {
                itm.SetValue(GuidGenerator.Create());
            }
            #endregion

            var dbConnection = await _repository.GetDbContextAsync();
            await dbConnection.BulkInsertAsync(entityList);//批量添加操作

            //if (entityList != null)
            //{
            //    //数据同步-发布事件
            //    await _publisher.PublishAsync("InventoryQty.StockAccounts", new StockCountChangedEto
            //    { ChangedDate = entityList.FirstOrDefault().CreationTime, Product = entityList.Sum(_ => _.Qty).ToString() }
            //    , "WJP.Business.StockAccountsCallBack");
            //}

            return ApplicationConsts.SuccessStr;
        }

        /// <summary>
        /// 不使用Dapper，直接应用底层仓储调用SQL
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public virtual async Task<List<BomDto>> GetListAsync()
        {
            //List<BomDto> list = null; 
            //var bulk = _dicRepository.BulkInsertAsync(list);//批量添加

            var database = (await _repository.GetDbContextAsync()).Database;
            var dbConnection = database.GetDbConnection();
            var dbTransaction = database.CurrentTransaction?.GetDbTransaction();

            var queryResult = await dbConnection.QueryAsync<BomDto>(
                "SELECT *  FROM [SettleAccount_Module].[dbo].[SettleAccount_bom]",
                transaction: dbTransaction
            );

            return queryResult.ToList();
        }


        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("Export")]
        ////[Authorize(SettleAccountPermissions.Boms.Default)]
        //virtual public async Task<string> ExportAsync(BomRequestDto input)
        //{
        //    IExporter _csv = new CsvExporter();
        //    IExporter _excel = new ExcelExporter();
        //    //导出加上版本过滤条件，不能全导出
        //    if (input.ParentId != Guid.Empty)
        //    {
        //        input.Filters.Add(new FilterCondition() { Action = EnumFilterAction.Equal, Column = "ParentId", Logic = EnumFilterLogic.And, Value = input.ParentId.ToString() });
        //    }
        //    //else
        //    //{
        //    //    return new PagedResultDto<BomDto>(0, new List<BomDto>());
        //    //}
        //    var entities = await _repository.GetListByFilterAsync(input.BranchId, input.Filters, input.Sorting, int.MaxValue,
        //       0, true);
        //    var dtoDetails = ObjectMapper.Map<List<Bom>, List<BomExportDto>>(entities);
        //    string _fileName = string.Empty;
        //    //声明导出容器

        //    byte[] result = null;
        //    switch (input.FileType)
        //    {
        //        case 0:
        //            _fileName = string.Format("产品结构_{0}.csv", input.UserId.ToString());
        //            result = await _csv.ExportAsByteArray(dtoDetails);
        //            break;
        //        case 1:
        //            _fileName = string.Format("产品结构_{0}.xlsx", input.UserId.ToString());
        //            result = await _excel.ExportAsByteArray(dtoDetails);
        //            break;
        //    }
        //    result.ShouldNotBeNull();

        //    //保存导出文件到服务器存成二进制 
        //    await _fileStorageBlobAppService.SaveBlobAsync(
        //           new SaveFileBlobInputDto
        //           {
        //               Name = _fileName,
        //               Content = result
        //           }
        //       );
        //    return _fileName;
        //}
        //#endregion

        ///// <summary>
        ///// 按ID获取唯一实体
        ///// </summary>
        ///// <remarks>
        ///// 返回实体全部属性
        ///// </remarks>
        ///// <param name="id">ID</param>
        ///// <returns>实体DTO</returns>
        //[HttpGet]
        //[Route("{id}")]
        //[Authorize(SettleAccountPermissions.Boms.Default)]
        //virtual public async Task<BomDto> GetAsync(Guid id)
        //{
        //    var result = await _repository.GetAsync(id);
        //    var dto = _objectMapper.Map<Bom, BomDto>(result);
        //    return dto;
        //}


        //private async Task<Bom> GetFromCacheAsync(Guid id)
        //{
        //    var result = await _repository.GetAsync(id);
        //    return result;
        //}


        //private async Task<long> GetCountAsync(BomRequestDto input)
        //{
        //    return await _repository.GetCountByFilterAsync(input.BranchId, input.Filters);
        //}


        //private async Task<long> GetCountAsync(BomVersionRequestDto input)
        //{
        //    return await _versionRepository.GetCountByFilterAsync(input.BranchId, input.Filters);
        //}

        ///// <summary>
        ///// 获取实体总数
        ///// </summary>
        ///// <returns>实体总数</returns>
        //[HttpGet]
        //[Route("count")]
        //[Authorize(SettleAccountPermissions.Boms.Default)]
        //virtual public async Task<long> GetTotalCountAsync(Guid branchId)
        //{
        //    return await _repository.GetCountAsync(branchId);
        //}



        ///// <summary>
        ///// 删除实体
        ///// </summary>
        ///// <param name="id">ID</param>
        ///// <returns>无</returns>
        //[HttpDelete]
        //[Route("{id}")]
        //[Authorize(SettleAccountPermissions.Boms.Delete)]
        //virtual public async Task DeleteAsync(Guid id)
        //{
        //    //var entity = await GetFromCacheAsync(id);
        //    //await Cache.DeleteAsync<Bom>(id.ToString());
        //    await _repository.DeleteAsync(id);
        //}

        ///// <summary>
        ///// 按IDs删除实体列表
        ///// </summary>
        ///// <param name="ids">IDs</param>
        ///// <returns>是否执行成功</returns>
        //[HttpPost]
        //[Route("delete")]
        //[Authorize(SettleAccountPermissions.Boms.Delete)]
        //virtual public async Task<bool> DeleteListAsync(List<Guid> ids)
        //{
        //    //foreach (var id in ids)
        //    //{
        //    //    var entity = await GetFromCacheAsync(id);

        //    //}

        //    return await _repository.DeleteListAsync(ids);
        //}


        /////// <summary>

        ///// <summary>
        ///// 根据筛选条件获取实体列表
        ///// </summary>
        ///// <remarks>
        ///// 请求条件包括:筛选条件列表,排序条件,数据数量,页码
        ///// </remarks>
        ///// <param name="input">请求条件</param>
        ///// <returns>实体DTO列表</returns>
        //[HttpPost]
        //[Route("list")]
        //[Authorize(SettleAccountPermissions.Boms.Default)]
        //public async Task<PagedResultDto<BomDto>> GetListAsync(Guid parentId, BomRequestDto input)
        //{
        //    if (input.ParentId != Guid.Empty)
        //    {
        //        input.Filters.Add(new FilterCondition() { Action = EnumFilterAction.Equal, Column = "ParentId", Logic = EnumFilterLogic.And, Value = input.ParentId.ToString() });
        //    }
        //    else
        //    {
        //        return new PagedResultDto<BomDto>(0, new List<BomDto>());
        //    }
        //    var entities = await _repository.GetListByFilterAsync(input.BranchId, input.Filters, input.Sorting, input.MaxResultCount,
        //         input.SkipCount, true);
        //    var totalCount = await GetCountAsync(input);
        //    var dtos = _objectMapper.Map<List<Bom>, List<BomDto>>(entities);
        //    return new PagedResultDto<BomDto>(totalCount, dtos);
        //}


    }
}
