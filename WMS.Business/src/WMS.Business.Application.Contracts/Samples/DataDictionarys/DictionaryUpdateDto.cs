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
    /// 修改DTO
    /// </summary>
    public class DictionaryUpdateDto : AbpCreateOrUpdateInputBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
