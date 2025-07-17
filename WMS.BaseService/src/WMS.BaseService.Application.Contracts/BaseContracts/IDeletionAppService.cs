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
    public interface IDeletionAppService<T> : IApplicationService, IRemoteService where T : DeleteEntityDto
    {
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(DeleteEntityDto input);
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        Task<bool> DeleteManyAsync(List<DeleteEntityDto> inputs);
    }
}
