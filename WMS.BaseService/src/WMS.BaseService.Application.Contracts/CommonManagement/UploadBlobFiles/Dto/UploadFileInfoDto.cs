using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.BaseService.CommonManagement.UploadBlobFiles;
using Volo.Abp.Application.Dtos;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles.Dto
{
    public class UploadFileInfoDto : EntityDto<Guid>
    {
        public DateTime CreationTime { get; set; }

        [Display(Name = "附件名称")]
        [StringLength(500)]
        [Required(ErrorMessage = "{0}是必填项")]
        public string FileName { get; set; }

        [Display(Name = "附件路径")]
        [Required(ErrorMessage = "{0}是必填项")]
        public string FileUrl { get; set; }

        [Display(Name = "附件长度")]
        public long? Size { get; set; }

        [Display(Name = "附件类型")]
        public FileType Type { get; set; }

        [Display(Name = "附件后缀")]
        public string Suffix { get; set; }

        [Display(Name = "模块附件Id")]
        public string BaseDataId { get; set; }

        [Display(Name = "模块名称（即此附件所属模块）")]
        public string BaseDataName { get; set; }
    }
}
