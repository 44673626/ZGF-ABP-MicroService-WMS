using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles.Dto
{
    public class BlobFilesDto : Entity<Guid>
    {
        public byte[] Content { get; set; }

        public string Name { get; set; }
    }
}
