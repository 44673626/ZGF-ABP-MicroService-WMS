﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win.Sfs.FileStorage.UploadFile.Dto
{
    public class GetBlobFileRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
