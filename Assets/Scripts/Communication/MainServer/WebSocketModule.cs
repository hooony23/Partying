using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Util;
using Lib;
using Newtonsoft.Json.Linq;
namespace Communication.MainServer
{
    public class WebSocketModule : MonoBehaviour
    {
        public void Start()
        {

            string chatHubUrl = Config.mainServerDNS + "/" + "rooms";
            var connection = new HubConnectionBuilder()
                .WithUrl(chatHubUrl)
                .WithAutomaticReconnect()
                .Build();

            connection.On<string>("receive", data =>
            {
                Console.WriteLine($"receive: {data}");
            });

            connection.On<string>("roomList", data =>
            {
                NetworkInfo.roomList = Common.GetData(data)["roomList"] as JArray;
            });

            connection.On<string>("memberInfo", data =>
            {
                NetworkInfo.memberInfo = Common.GetData(data)["memberInfo"] as JArray;
            });

            connection.StartAsync();
        }
    }
}
