using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.CAP.EtoShare
{
    public class Cap_ChangPasswordEto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        public Cap_ChangPasswordEto(string loginName, string password)
        {
            LoginName = loginName;
            Password = password;
        }
    }
}
