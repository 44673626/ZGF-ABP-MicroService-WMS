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
    public interface ICreationAppService<T, TResult> : IApplicationService, IRemoteService where T : CreateEntityDto where TResult : ResultEntityDto
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResultEntityDto> CreateAsync<ResultEntityDto>(CreateEntityDto input);
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task<bool> CreateManyAsync(List<CreateEntityDto> inputs);
    }
}
