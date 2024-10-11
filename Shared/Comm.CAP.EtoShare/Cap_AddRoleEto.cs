using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.CAP.EtoShare
{
    public class Cap_AddRoleEto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string Role_Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        public Cap_AddRoleEto(string roleName, string role_Id = null)
        { 
            RoleName = roleName; 
            Role_Id = role_Id;
        }
    }

}
