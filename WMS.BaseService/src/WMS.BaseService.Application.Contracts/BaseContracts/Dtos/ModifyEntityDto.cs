using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WMS.BaseService.BaseContracts.Dtos
{
    public class ModifyEntityDto : EntityDto<Guid>
    {
        //public string ConcurrencyStamp { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
