using BaseService.Controllers;
using BaseService.Systems.MessageManagement;
using BaseService.Systems.NoticesManagement.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace BaseService.HttpApi.Systems
{
    [Area("base")]
    [Route("api/base/signalR")]
    //[ApiExplorerSettings(GroupName = "v1")]
    public class SignalController: BaseServiceController
    {
        public static readonly List<OnlineUsers> onlineClients = new();
        private readonly IHubContext<MessageHub> _countHub;
        public SignalController(IHubContext<MessageHub> countHub)
        {
            _countHub = countHub;
        }
        [HttpGet]
        public async Task OnlineUsers()
        {
            var user = CurrentUser;
            if (user != null && user.Id != null)
            {
                var onlineUser = new OnlineUsers()
                {
                    ConnnectionId = user.Id.Value.ToString(),
                    UserId = user.Id.Value,
                    Name = user.Name,
                    LoginTime = DateTime.Now

                };
                onlineClients.Add(onlineUser);
            }
            //给所有用户更新在线人数
            await _countHub.Clients.All.SendAsync(HubsConstant.OnlineNum, new
            {
                num = onlineClients.Count,
                onlineClients
            });
        }
        /// <summary>
        /// 送给指定用户或者所有用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SendMessageToUsers")]
        public async Task SendMessageToUsersAsync(List<string> userIds, CreateNoticeInput notice)
        {
            await _countHub.Clients
                .Users(userIds.AsReadOnly().ToList())
                .SendAsync(HubsConstant.ReceiveNotice, notice.Title, notice.NoticeContent);
        }
        [HttpGet]
        [Route("SendMessageToAll")]
        public async Task SendMessageToAllAsync(CreateNoticeInput notice)
        {
            await _countHub.Clients.All.SendAsync(HubsConstant.ReceiveNotice, notice.Title, notice.NoticeContent);
        }

  


    }
}
