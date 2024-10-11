using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.CAP.EtoShare
{
    public class Cap_RemoveRoleEto
    {
        /// <summary>
        /// 要删除的角色名称
        /// </summary>
        public string RoleName { get; set; }

        public Cap_RemoveRoleEto(string roleName)
        {
            RoleName = roleName;
        }
    }
}
