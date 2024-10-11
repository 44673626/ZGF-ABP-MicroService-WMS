using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BaseService.BaseData.CompanyManagement.Dto
{
    public class GetCompanyInputDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
