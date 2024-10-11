using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FileStorage.UploadFile.Dto
{
    /// <summary>
    /// 根据类型查附件
    /// </summary>
    public class GetFilesOfDataIDInputDto : PagedAndSortedResultRequestDto
    {
        [Required]
        public string BaseDataId { get; set; }
    }
}
