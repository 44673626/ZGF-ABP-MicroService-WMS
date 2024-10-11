using BaseService.Systems.MessageManagement;
using BaseService.Systems.NoticesManagement.Input;
using BaseService.Systems.NoticesManagement.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace BaseService.Systems.NoticesManagement
{
    public interface INoticeService : IApplicationService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PageResponseResult<NoticeOutput>> GetPageListAsync(NoticePageListInput input);
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseResult<List<NoticeOutput>>> GetAllAsync();
        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseResult<NoticeOutput>> GetAsync(Guid id);
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseResult<NoticeOutput>> CreateAsync(CreateNoticeInput input);
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseResult<NoticeOutput>> UpdateAsync(UpdateNoticeInput input);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseResult> DeleteAsync(IdsInput<Guid> input);


        Task<ResponseResult> SendNotice(Guid id);
    }
}
