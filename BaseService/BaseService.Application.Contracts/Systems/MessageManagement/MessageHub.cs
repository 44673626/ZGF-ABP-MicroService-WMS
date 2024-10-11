using BaseService.Systems.NoticesManagement.Output;
using BaseService.Systems.NoticesManagement;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace BaseService.Systems.MessageManagement
{
    /// <summary>
    /// 统计在线人数
    /// </summary>
    //[HubRoute("api/v1/sys/signalr-hubs")]
    public class MessageHub : AbpHub
    {
        //创建用户集合，用于存储所有链接的用户数据
        public static readonly List<OnlineUsers> onlineClients = new();
        public static List<OnlineUsers> users = new();
        private readonly IAbpLazyServiceProvider _provider;
        public MessageHub(IAbpLazyServiceProvider provider)
        {
            _provider = provider;
        }
        /// <summary>
        /// 客户端连接的时候调用
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            try
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
                    //Clients.Caller.SendAsync(HubsConstant.MoreNotice, SendNotice().Result);
                    Clients.Caller.SendAsync(HubsConstant.ConnId, onlineUser.ConnnectionId);
                }

                //给所有用户更新在线人数
                Clients.All.SendAsync(HubsConstant.OnlineNum, new
                {
                    num = onlineClients.Count,
                    onlineClients
                });
                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 连接终止时调用。
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (CurrentUser == null || CurrentUser.Id == null)
            {
                return Task.CompletedTask;
            }
            var user = onlineClients.Where(p => p.ConnnectionId == CurrentUser.Id.Value.ToString()).FirstOrDefault();
            if (user != null)
            {
                onlineClients.Remove(user);
                //给所有用户更新在线人数
                Clients.All.SendAsync(HubsConstant.OnlineNum, new
                {
                    num = onlineClients.Count,
                    onlineClients,
                    leaveUser = user
                });


            }


            return base.OnDisconnectedAsync(exception);
        }
        private async Task<ResponseResult<List<NoticeOutput>>> SendNotice()
        {
            var NoticeService = _provider.LazyGetRequiredService<INoticeService>();
            var list = await NoticeService.GetAllAsync();
            return ResponseResult<List<NoticeOutput>>.Success("ok", list.Result);
        }

        [HubMethodName("sendone")]
        public void HubTextMsg(string message, string connectionid)
        {

            //if (connectionid != null)
            //{
            //    //调用客户端方法
            //    Clients.Client(connectionid).SendMessage("ID:" + connectionid, message + " 时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //}
            //else
            //{
            //    Clients.All.SendMessage("ID:" + connectionid, message + " 时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //}
        }

        /// <summary>
        /// 发送消息到指定分组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }


        /// <summary>
        /// 添加到分组
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync("ConnectionId", groupName);
        }



        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="toUserId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HubMethodName("sendMessage")]
        public async Task SendMessage(Guid toUserId, string message)
        {
            if (CurrentUser == null)
            {
                return;
            }
            var userName = CurrentUser.UserName;
            Guid userid = CurrentUser.Id.Value;
            var toUserList = onlineClients.Where(p => p.UserId == toUserId);
            var toUserInfo = toUserList.FirstOrDefault();
            IList<string> sendToUser = toUserList.Select(x => x.ConnnectionId).ToList();
            sendToUser.Add(userid.ToString());


            ChatMessageDto messageDto = new()
            {
                MsgType = 0,
                StoredKey = $"{userid}-{toUserId}",
                UserId = userid,
                ChatId = Guid.NewGuid().ToString(),
                ToUserId = toUserId,
                Message = message,
                Online = 1,
                ChatTime = DateTime.Now,
                FromUser = new ChatUserDto() { UserName = userName },
            };
            if (toUserInfo == null)
            {
                messageDto.Online = 0;
                //TODO 存储离线消息
                Console.WriteLine($"{toUserId}不在线");
            }
            else
            {
                await Clients.Clients(sendToUser)
                       .SendAsync("receiveChat", messageDto);
            }
        }
    }

    public class HubsConstant
    {
        private const string V = "receiveNotice";
        public static string ReceiveNotice = V;
        public static string OnlineNum = "onlineNum";
        public static string MoreNotice = "moreNotice";
        public static string OnlineUser = "onlineUser";
        public static string LockUser = "lockUser";
        public static string ForceUser = "forceUser";
        public static string ConnId = "connId";
    }
}
