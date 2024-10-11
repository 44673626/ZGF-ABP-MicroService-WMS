using ABP.Business.Settings;
using AutoMapper.Internal.Mappers;
using ABP.Business.CommonManagement.Bases;
using ABP.Business.Samples.DataDictionarys;
using ABP.Business.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace ABP.Business.Samples.Dictionarys
{
    /// <summary>
    /// 操作一维表的基础功能示例（添加、修改、删除和动态条件的过滤）
    /// </summary>
    [Area(ABPVNextRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = ABPVNextRemoteServiceConsts.RemoteServiceName)]
    [Route($"{ApiConsts.RootPath}DataDictionary")]
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class DictionaryAppService :
        AbpCrudAppServiceBase<DataDictionary, DictionaryDto, Guid, 
            DictionaryRequestDto, DictionaryCreateDto, DictionaryUpdateDto>,
        IDictionaryAppService
    {
        public DictionaryAppService(IDictionaryRepository repository, 
            IDistributedCache<DictionaryDto> cache) : base(repository, cache)
        {
        }
        /// <summary>
        /// 批量删除操作
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public async Task Delete(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                await _repository.DeleteAsync(id);
            }
        }
    }
}
