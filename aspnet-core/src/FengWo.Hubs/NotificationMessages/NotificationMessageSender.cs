using Abp;
using Abp.Dependency;
using Abp.RealTime;
using Castle.Core.Logging;
using FengWo.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FengWo.Hubs.NotificationMessages
{
    public class NotificationMessageSender: ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationMessageSender(IOnlineClientManager onlineClientManager,
            IHubContext<ChatHub> hubContext)
        {
            _onlineClientManager = onlineClientManager;
            _hubContext = hubContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void PleaseGetNotificationMessage()
        {
            //foreach (var user in users)
            //{
            //    var clients = _onlineClientManager.GetAllClients().Where(p => p.UserId == user.UserId).ToList();//.GetAllByUserId(user)
            //    foreach (var item in clients)
            //    {
            //        var client = _hubContext.Clients.Client(item.ConnectionId);
            //        if(client!=null)
            //        {
            //            client.SendAsync("getMessage", "123");
            //        }
            //    }
            //}
            _hubContext.Clients.All.SendAsync("getNotificationMessage");
        }
    }
}
