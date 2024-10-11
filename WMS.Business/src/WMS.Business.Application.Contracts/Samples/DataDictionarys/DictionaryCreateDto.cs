using WMS.Business.CommonManagement.Crud.Inputs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.Samples.DataDictionarys
{
    /// <summary>
    /// 创建DTO
    /// </summary>
    public class DictionaryCreateDto : AbpCreateOrUpdateInputBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
