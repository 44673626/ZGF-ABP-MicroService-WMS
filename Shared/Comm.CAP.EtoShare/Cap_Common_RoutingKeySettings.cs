using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.CAP.EtoShare
{
    /// <summary>
    /// 设置业务系统的路由键
    /// </summary>
    public static class Cap_Business_RoutingKeySettings
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        public const string Bus_AddUser = "Comm:AddUser@IMMP";
        /// <summary>
        /// 删除用户
        /// </summary>
        public const string Bus_RemoveUser = "Comm:RemoveUser@IMMP";

        /// <summary>
        /// 修改用户
        /// </summary>
        public const string Bus_ModifyUser = "Comm:ModifyUser@IMMP";

        /// <summary>
        /// 修改用户密码
        /// </summary>
        public const string Bus_ChangePassword= "Comm:ChangePassword@IMMP";

        /// <summary>
        /// 添加角色
        /// </summary>
        public const string Bus_AddRole = "Comm:AddRole@IMMP";

        /// <summary>
        /// 修改角色
        /// </summary>
        public const string Bus_ModifyRole = "Comm:ModifyRole@IMMP";

        /// <summary>
        /// 删除角色
        /// </summary>
        public const string Bus_RemoveRole = "Comm:RemoveRole@IMMP";

        /// <summary>
        /// 批量同步用户信息的差异数据
        /// </summary>
        public const string SyncUsers = "Comm:SyncUsers@IMMP";
        /// <summary>
        /// 批量同步角色信息的差异数据
        /// </summary>
        public const string SyncRoles = "Comm:SyncRoles@IMMP";


    }
}
