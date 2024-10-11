using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WMS.BaseService.CommonManagement.Crud.Inputs
{
    public interface
        IAbpCrudAppService<TEntityDTO, in TKey, in TRequestInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDTO, TKey, TRequestInput, TCreateInput, TUpdateInput>
        , IAbpCustomRequestAppService<TEntityDTO, TRequestInput>
    {


    }
}
