using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Win.Sfs.FileStorage.UploadFile.Dto
{
    public class BlobImgsDto : Entity<Guid>
    {
        public byte[] Content { get; set; }

        public string Name { get; set; }
    }
}
