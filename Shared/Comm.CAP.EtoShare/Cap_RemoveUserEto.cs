using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.CAP.EtoShare
{
    /// <summary>
    /// 删除用户信息
    /// </summary>
    public class Cap_RemoveUserEto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        public Cap_RemoveUserEto(string loginName)
        {
            LoginName = loginName;
        }
    }
}
