using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABP.Business.CommonManagement.Crud.Inputs;

namespace ABP.Business.CommonManagement.Crud
{
    public interface IAbpBaseCrudAppService<TEntityDTO, in TKey, in TRequestInput, in TCreateInput, in TUpdateInput>
       : IAbpCrudAppService<TEntityDTO, TKey, TRequestInput, TCreateInput, TUpdateInput>
    {
    }
}
