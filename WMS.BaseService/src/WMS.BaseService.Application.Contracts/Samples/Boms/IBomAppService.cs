using WMS.BaseService.Samples.Boms.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.Samples.Boms
{
    public interface IBomAppService
    {

        /// <summary>
        /// 导入功能
        /// </summary>
        /// <param name="files">上传的文件(前端已经限制只能上传一个附件)</param>
        /// <returns></returns>

        Task<string> BomUploadExcelImport([FromForm] IFormFileCollection files);
        // [FromForm] string year, [FromForm] string period, [FromForm] string version, [FromForm] string customerCode, [FromForm] string factory);

        Task<List<BomDto>> GetListAsync();


        /// <summary>
        /// 按ID获取唯一实体
        /// </summary>
        /// <remarks>
        /// 返回实体全部属性
        /// </remarks>
        /// <param name="id">ID</param>
        /// <returns>实体DTO</returns>

        //Task<BomDto> GetAsync(Guid id);

        /// <summary>
        /// 根据筛选条件获取实体列表
        /// </summary>
        /// <remarks>
        /// 请求条件包括:筛选条件列表,排序条件,数据数量,页码
        /// </remarks>
        /// <param name="input">请求条件</param>
        /// <returns>实体DTO列表</returns>

        //Task<PagedResultDto<BomDto>> GetListAsync(Guid parentId, BomRequestDto input);




        /// <summary>
        /// 根据筛选条件获取实体列表
        /// </summary>
        /// <remarks>
        /// 请求条件包括:筛选条件列表,排序条件,数据数量,页码
        /// </remarks>
        /// <param name="input">请求条件</param>
        /// <returns>实体DTO列表</returns>

        //Task<PagedResultDto<BomVersionDto>> GetVersionListAsync(BomVersionRequestDto input);




        /// <summary>
        /// 获取实体总数
        /// </summary>
        /// <returns>实体总数</returns>

        //Task<long> GetTotalCountAsync(Guid branchId);




        //Task<bool> DeleteListAsync(List<Guid> ids);
    }
}
