using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp;
using WMS.BaseService.BaseContracts.Dtos;

namespace WMS.BaseService.BaseContracts
{
    /// <summary>
    /// 基础接口
    /// </summary>
    public interface IRequestAppService<TRequst, TResult> : IApplicationService, IRemoteService where TRequst : RequestEntityDto where TResult : ResultEntityDto
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ResultEntityDto>> GetListAsync<ResultEntityDto>(RequestEntityDto input);
        /// <summary>
        /// 查询明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(Guid id);
    }
}
