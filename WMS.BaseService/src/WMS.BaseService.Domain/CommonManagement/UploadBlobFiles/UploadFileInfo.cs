using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles
{
    public class UploadFileInfo : AuditedAggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {

        [Display(Name = "租户Id")]
        public Guid? TenantId { get; set; }

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

        public string Md5Code { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "模块附件Id")]
        public string BaseDataId { get; set; }

        [Display(Name = "模块名称（即此附件所属模块）")]
        public string BaseDataName { get; set; }

        private UploadFileInfo()
        {

        }

        public UploadFileInfo(Guid id, Guid? tenantId, string filename, string suffix, string md5code, string url, FileType type, string basedataId, string basedataName, long? size) : base(id)
        {
            TenantId = tenantId;
            FileName = filename;
            Suffix = suffix;
            Md5Code = md5code;
            FileUrl = url;
            Type = type;
            BaseDataId = basedataId;
            BaseDataName = basedataName;
            Size = size;
        }
    }
}
