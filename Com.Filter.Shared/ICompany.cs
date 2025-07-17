using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Filter.Shared
{
    public interface ICompany
    {
        Guid? CompanyId { get; set; }

        void SetCompanyId(string companyId);
    }
}
