using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace WMS.BaseService.BaseContracts.Dtos
{
    public class DeleteEntityDto : EntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}
