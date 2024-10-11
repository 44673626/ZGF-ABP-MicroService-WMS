using BaseService.Controllers;
using BaseService.Systems.UserManagement;
using COM.CAP.EtoShare;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseService.HttpApi.Systems
{
    [Route("api/cap/user")]
    //[ApiExplorerSettings(GroupName = "v1")]
    public class CAPUserController : BaseServiceController, ICapSubscribe
    {
        //接口注入IOC容器中
        private readonly ICAPUserAppService _capUserAppService;

        public CAPUserController(ICAPUserAppService capUserAppService)
        {
            _capUserAppService = capUserAppService;
        }

        /// <summary>
        /// CAP：同步接收过来的用户信息
        /// </summary>
        /// <param name="eto">同步接收的事件传输数据</param>
        /// <returns></returns>
        [HttpGet]
        [Route("add/user")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_AddUser)]
        public async Task CreateUserAsync(Cap_AddOrModifyUserEto eto)
        {
            await _capUserAppService.CreateUserAsync(eto);
        }

        /// <summary>
        /// CAP：同步修改用户的信息
        /// </summary>
        /// <param name="eto">同步接收的事件传输数据</param>
        /// <returns></returns>
        [HttpGet]
        [Route("modify/user")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_ModifyUser)]
        public async Task UpdateUserAsync(Cap_AddOrModifyUserEto eto)
        {
            await _capUserAppService.UpdateUserAsync(eto);
        }

        /// <summary>
        /// CAP:同步删除用户信息
        /// </summary>
        /// <param name="eto">同步接收的事件传输数据</param>
        /// <returns></returns>
        [HttpGet]
        [Route("remove/user")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_RemoveUser)]
        public async Task DeleteUserAsync(Cap_RemoveUserEto eto)
        {
            await _capUserAppService.DeleteUserAsync(eto);
        }

        /// <summary>
        /// CAP:修改用户密码
        /// </summary>
        /// <param name="eto">同步接收的事件传输数据</param>
        /// <returns></returns>
        [HttpGet]
        [Route("changepassword/user")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_ChangePassword)]
        public async Task ResetPasswordAsync(Cap_ChangPasswordEto eto)
        {
            await _capUserAppService.ResetPasswordAsync(eto);
        }

        /// <summary>
        /// CAP:同步角色的新增数据
        /// </summary>
        /// <param name="eto">同步接收的事件传输数据</param>
        /// <returns></returns>
        [HttpGet]
        [Route("add/role")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_AddRole)]
        public async Task CreateRoleAsync(Cap_AddRoleEto eto)
        {
            await _capUserAppService.CreateRoleAsync(eto);
        }

        /// <summary>
        /// CAP:同步角色的修改信息
        /// </summary>
        /// <param name="eto">同步接收的事件传输数据</param>
        /// <returns></returns>
        [HttpGet]
        [Route("modify/role")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_ModifyRole)]
        public async Task UpdateRoleAsync(Cap_ModdifyRoleEto eto)
        {
            await _capUserAppService.UpdateRoleAsync(eto);
        }

        /// <summary>
        /// CAP:同步删除角色信息
        /// </summary>
        /// <param name="eto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("remove/role")]
        [CapSubscribe(Cap_Business_RoutingKeySettings.Bus_RemoveRole)]
        public async Task DeteleRoleAsync(Cap_RemoveRoleEto eto)
        {
            //string roleName = eto.RoleName;
            await _capUserAppService.DeteleRoleAsync(eto);
        }


    }
}
