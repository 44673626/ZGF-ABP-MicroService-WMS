using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.CAP.EtoShare
{
    public class Cap_ModdifyRoleEto
    {
        /// <summary>
        /// 原角色名称
        /// </summary>
        public string OriginalRoleName { get; set; }

        /// <summary>
        /// 新角色名称
        /// </summary>
        public string ChangedRoleName { get; set; }

        public Cap_ModdifyRoleEto(string originalRoleName, string changedRoleName)
        {
            OriginalRoleName = originalRoleName;
            ChangedRoleName = changedRoleName;
        }
    }
}
