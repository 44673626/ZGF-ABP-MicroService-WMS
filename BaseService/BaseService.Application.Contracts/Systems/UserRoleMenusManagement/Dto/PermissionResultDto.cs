using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.Systems.UserRoleMenusManagement.Dto
{
    public class PermissionResultDto
    {
        public Dictionary<string, bool> GrantedPolicies { get; set; }

        public PermissionResultDto()
        {
            GrantedPolicies = new Dictionary<string, bool>();
        }
    }
}
