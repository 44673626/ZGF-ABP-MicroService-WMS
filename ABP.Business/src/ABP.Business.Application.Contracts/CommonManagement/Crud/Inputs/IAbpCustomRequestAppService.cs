using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ABP.Business.CommonManagement.Crud.Inputs
{
    public interface IAbpCustomRequestAppService<TEntityDTO, in TRequestInput>
    {
        /// <summary>
        /// 获取符合条件的数据条数
        /// </summary>
        /// <param name="sfsRequestDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CountAsync(TRequestInput sfsRequestDTO, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取符合条件的数据列表
        /// </summary>
        /// <param name="sfsRequestDTO"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedResultDto<TEntityDTO>> GetListAsync(TRequestInput sfsRequestDTO, bool includeDetails = false,
            CancellationToken cancellationToken = default);



        /// <summary>
        /// 获取符合条件的数据列表
        /// </summary>
        /// <param name="sfsRequestDTO"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TEntityDTO>> GetAllListAsync(TRequestInput sfsRequestDTO, bool includeDetails = false,
            CancellationToken cancellationToken = default);



        Task<PagedResultDto<TEntityDTO>> SearchAsync(string keyWord, int skipCount, int maxResultCount, string sorting,
            bool includeDetails = false, CancellationToken cancellationToken = default);



    }
}
