using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WMS.Business.Samples.DataDictionarys
{
    /// <summary>
    /// 实体对应的DTO
    /// </summary>
    public class DictionaryDto : EntityDto<Guid>
    {
        /// <summary>
        /// 字典名称 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字典描述
        /// </summary>
        public string Description { get; set; }
    }
}
