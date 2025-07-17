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
    public interface IModifyAppService<T, TResult> : IApplicationService, IRemoteService where T : ModifyEntityDto where TResult : ResultEntityDto
    {
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResultEntityDto> ModifyAsync<ResultEntityDto>(ModifyEntityDto input);
        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task<bool> ModifyManyAsync(List<ModifyEntityDto> inputs);
    }
}
