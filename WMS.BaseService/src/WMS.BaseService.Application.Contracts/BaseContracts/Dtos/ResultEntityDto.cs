using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace WMS.BaseService.BaseContracts.Dtos
{
    public class ResultEntityDto : EntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        public Guid? LastModifierId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string LastModifier { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
    }
}

