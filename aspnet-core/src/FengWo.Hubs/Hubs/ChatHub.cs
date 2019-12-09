using Abp;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.RealTime;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using FengWo.Authorization.Users;
using FengWo.Web.Hubs.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FengWo.Web.Hubs
{
    public class ChatHub : AbpHubBase, ITransientDependency
    {
        public IAbpSession AbpSession { get; set; }

        public ILogger Logger { get; set; }
//        private readonly 

        public ChatHub(IOnlineClientManager onlineClientManager)
        {
            AbpSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("getMessage", string.Format("User"));
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {

            return base.OnDisconnectedAsync(exception);
        }
    }
}
