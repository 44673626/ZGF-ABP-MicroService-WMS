using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp;
using WMS.BaseService.BaseContracts.Dtos;

namespace WMS.BaseService.BaseContracts
{
    /// <summary>
    /// 基础接口
    /// </summary>
    public interface IRequestPageAppService<TRequst, TResult> : IApplicationService, IRemoteService where TRequst : RequestPageEntityDto where TResult : ResultEntityDto
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ResultEntityDto>> GetPageListAsync<ResultEntityDto>(RequestPageEntityDto input);
    }
}
