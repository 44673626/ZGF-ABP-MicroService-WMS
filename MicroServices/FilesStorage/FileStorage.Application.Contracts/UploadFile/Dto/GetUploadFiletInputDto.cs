using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace FileStorage.UploadFile.Dto
{
    /// <summary>
    /// 过滤附件列表
    /// </summary>
    public class GetUploadFiletInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
