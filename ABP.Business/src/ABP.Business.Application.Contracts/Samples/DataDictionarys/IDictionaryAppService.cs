using ABP.Business.CommonManagement.Crud.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ABP.Business.Samples.DataDictionarys
{
    /// <summary>
    /// 继承CURD接口
    /// </summary>
    public interface IDictionaryAppService
        : IAbpCrudAppService<DictionaryDto, Guid, 
            DictionaryRequestDto, DictionaryCreateDto, DictionaryUpdateDto>
    {
        //自定义方法
        Task Delete(List<Guid> ids);//批量删除
    }
}
