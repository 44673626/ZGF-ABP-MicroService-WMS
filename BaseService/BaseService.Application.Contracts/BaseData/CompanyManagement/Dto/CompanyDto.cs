using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BaseService.BaseData.CompanyManagement.Dto
{
    public class CompanyDto : EntityDto<Guid>
    {
        /// <summary>
        /// 公司编码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 公司名称 
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 是否是总公司
        /// </summary>
        public bool WhethertoHeadOffice { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 启用日期
        /// </summary>
        public DateTime RestartDate { get; set; }
        /// <summary>
        /// 启用原因
        /// </summary>
        public string RestartReason { get; set; }
        /// <summary>
        /// 停用日期
        /// </summary>
        public DateTime? DeactivationDate { get; set; }
        /// <summary>
        /// 停用原因
        /// </summary>
        public string DeactivationReason { get; set; }
        /// <summary>
        /// 删除原因
        /// </summary>
        public string DeletionReason { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? Pid { get; set; }
    }
}
