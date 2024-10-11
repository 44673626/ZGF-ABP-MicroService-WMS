using WMS.Business.CommonManagement;
using WMS.Business.CommonManagement.Bases;
using WMS.Business.Samples.DataDictionarys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;


namespace WMS.Business.BaseData
{
    //路由可以加在Application服务层，此处的控制器可以不用再写代码，如果还想使用
    //，直接继承封装好的HxAbpControllerBase即可
    [Area(ABPVNextRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = ABPVNextRemoteServiceConsts.RemoteServiceName)]
    [Route("api/ABPVNext/DataDictionary")]
    [ApiExplorerSettings(GroupName = "Infra", IgnoreApi = false)]
    public class DictionaryController :
        AbpAbpControllerBase<DictionaryDto, Guid, 
            DictionaryRequestDto, DictionaryCreateDto, DictionaryUpdateDto>,
        IDictionaryAppService
    {
        private readonly IDictionaryAppService _dictionaryAppService;

        public DictionaryController(IDictionaryAppService dictionaryAppService) 
            : base(dictionaryAppService)
        {
            _dictionaryAppService = dictionaryAppService;
        }
        /// <summary>
        /// 批量删除操作（自定义方法）
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Delete")]
        public Task Delete(List<Guid> ids)
        {
            return _dictionaryAppService.Delete(ids);
        }

    }
}
