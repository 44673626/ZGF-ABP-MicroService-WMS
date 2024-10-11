using System;

namespace COM.CAP.EtoShare
{
    /// <summary>
    /// 事件传输出对象ETO
    /// </summary>
    public class Cap_AddOrModifyUserEto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        public string Enable { get; set; }

        public Cap_AddOrModifyUserEto(string userName,
            string loginName,
            string password,
            string mobile,
            string email,
            string userId = null,
            string roleId = null,
            string roleName = null,
            string enable = null
            )
        {
            UserName = userName;
            LoginName = loginName;
            Password = password;
            Mobile = mobile;
            Email = email;
            UserId = userId;
            RoleId = roleId;
            RoleName = roleName;
            Enable = enable;

        }
    }

}
