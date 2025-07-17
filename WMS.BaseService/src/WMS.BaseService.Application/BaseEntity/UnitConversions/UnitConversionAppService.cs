using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WMS.BaseService.BusinessEntity;

namespace WMS.BaseService.BaseEntity.UnitConversions
{
    public class UnitConversionAppService : WMSBaseAppService<UnitConversion>, IUnitConversionAppService
    {
        public UnitConversionAppService(IRepository<UnitConversion, Guid> repository) : base(repository)
        {
            //可添加自定义的服务方法
        }
    }
}
