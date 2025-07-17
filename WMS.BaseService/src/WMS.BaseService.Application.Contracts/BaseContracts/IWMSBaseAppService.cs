using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.BaseContracts.Dtos;

namespace WMS.BaseService.BaseContracts
{
    /// <summary>
    /// 封装基础接口
    /// </summary>
    public interface IWMSBaseAppService : IRequestAppService<RequestEntityDto, ResultEntityDto>,
        ICreationAppService<CreateEntityDto, ResultEntityDto>,
        IModifyAppService<ModifyEntityDto, ResultEntityDto>,
        IRequestPageAppService<RequestPageEntityDto, ResultEntityDto>,
        IDeletionAppService<DeleteEntityDto>
    {

    }
}
