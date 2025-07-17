using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.BaseService.BaseContracts.Dtos
{
    /// <summary>
    /// 停用/启用对象
    /// </summary>
    public class UsingStatusDto
    {
        /// <summary>
        /// 单据Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 停用/启用原因
        /// </summary>
        public string StatusChangeReason { get; set; }
    }
}
