using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Business.CommonManagement.UploadBlobFiles.Dto
{
    public class GetBlobFileRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
