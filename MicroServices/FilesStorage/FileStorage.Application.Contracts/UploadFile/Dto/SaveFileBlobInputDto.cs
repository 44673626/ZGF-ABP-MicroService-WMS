using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win.Sfs.FileStorage.UploadFile.Dto
{
    public class SaveFileBlobInputDto
    {
        public byte[] Content { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid TempId1 { get; set; }

        public Guid ContainerId { get; set; }
    }
}
