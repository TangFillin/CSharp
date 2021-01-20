using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public static string GroupName = "SignalR Group";
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }

        public override async Task OnConnectedAsync()
        {
            //Context获取当前登录用户信息
            var userName = Context.User.Claims.FirstOrDefault(c => c.Type.Equals("UserName")).Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);

            await Clients.Caller.SendAsync("ReceiveMessage", "系统消息", $"欢迎登录,{userName}！");

            
            await Clients.Others.SendAsync("ReceiveMessage", "系统消息", $"{userName} 已上线！");


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName);

            var userName = Context.User.Claims.FirstOrDefault(c => c.Type.Equals("UserName")).Value;
            await Clients.Others.SendAsync("ReceiveMessage", "系统消息", $"{userName} 已下线！");

            await base.OnDisconnectedAsync(exception);

        }

    }
}
