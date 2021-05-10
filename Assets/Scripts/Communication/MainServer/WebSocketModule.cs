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
                Debug.Log($"receive: {data}");
            });

            connection.On<string>("roomList", data =>
            {
                Debug.Log($"receive roomList : {data}");
                try{
                    NetworkInfo.roomList = Common.GetData(data)["roomList"] as JArray;
                }catch(Exception e)
                {
                    throw e;
                }
                
            });

            connection.On<string>("memberInfo", data =>
            {
                Debug.Log($"receive memberInfo : {data}");
                try{
                    NetworkInfo.memberInfo = Common.GetData(data)["memberInfo"] as JArray;
                }catch(Exception e)
                {
                    throw e;
                }
                
            });
            connection.On<string>("notifyConnectionId", data =>
            {
                try{
                    NetworkInfo.connectionId = Common.GetData(data)["connectionId"].ToString();
                }catch(Exception e)
                {
                    throw e;
                }
                Debug.Log($"receive connectionId : {data}");
            });
            connection.StartAsync();
        }
    }
}
