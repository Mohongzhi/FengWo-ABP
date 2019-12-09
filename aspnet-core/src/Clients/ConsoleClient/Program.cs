using FengWo.App.ClientLib;
using FengWo.App.Framework;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace ConsoleClient
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Clientlib.ServerUrl = "http://127.0.0.1:62114";
            //Clientlib.ServerUrl = "http://localhost:62114";

            Console.WriteLine("用户名:");
            var userName = Console.ReadLine();
            Console.WriteLine("密码:");
            var pwd = Console.ReadLine();

            Clientlib.Authenticate(userName, pwd, false);

            Console.WriteLine(Clientlib.UserId + "," + Clientlib.UserName);
            //Console.ReadKey();

            //var result = Clientlib.Authenticate("admin", "123qwe", false);

            HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:62114/signalr-chatHub").Build();
            connection.On<string>("getMessage", (msg) =>
            {
                Console.WriteLine("GetMessage:" + msg);
            });

            await connection.StartAsync();

            while (true)
            {               
                var line = Console.ReadLine();

                var data = new { UserId = Clientlib.UserId, UserName = Clientlib.UserName, MessageData = line, TargetUserId = 2 };

                connection.InvokeAsync("SendMessage", data);
            }

        }

    }
}
