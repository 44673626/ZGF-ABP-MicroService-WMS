using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WMS.Business.Samples.Boms.Dto
{
    public class BomRequestDto : PagedAndSortedResultRequestDto
    {
        public Guid ParentId { set; get; }

        public virtual Guid UserId { get; set; }

        public virtual int FileType { get; set; }


    }
}
