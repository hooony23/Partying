using System;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using Communication.JsonFormat;
using Newtonsoft.Json;

namespace Chatting
{
    public class ChatModule : MonoBehaviour
    {/*
        HubConnection connection = new HubConnectionBuilder()
            .WithUrl($"{Config.chatServerDNS}/chat")
            // .WithAutomaticReconnect()
            .Build();*/
        HubConnection connection = new HubConnectionBuilder()
                    .WithUrl($"https://skine134.iptime.org:42460/chat").Build();
        // static���� ����ð� ���� ����
        public string ReceiveData { get; set; } = null;
        static DateTime dateTime = DateTime.Now;
        static string currentTime = dateTime.ToString("HH:mm:ss");
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public async void Start()
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            connection.On<string>("ReceiveMessage", data =>
            {
                ReceiveData = data;
                Debug.Log(ReceiveData);
                Debug.Log(data);
            });
            await connection.StartAsync();
        }

        public void SendMsg(string message)
        {

            ChatInfo requestData = new ChatInfo();
            requestData.Nickname = "asdf";//myData.NickName;
            requestData.Message = message;
            var request = JsonConvert.SerializeObject(new { type = "SendMessage", uuid = Util.Config.userUuid, data = requestData });
            Debug.Log(request);
            connection.InvokeAsync("SendMessage", request.ToString());
        }
        /*BaseJsonFormat.ObjectToJson("signUp",requestData);
            connection.InvokeAsync("SendMessage", request.ToString());
            //Console.WriteLine($"###SendMessage complete### message: {message}");*/
        

        public void SendMessageToGroup(string message, string groupName = "main")
        {
            // NetworkInfo.roomInfo.RoomUuid;
            
            ChatInfo requestData = new ChatInfo();
            requestData.Nickname = "asdf";//myData.NickName;
            requestData.Message = message;
            JObject dataJson = JObject.FromObject(requestData);
            dataJson["groupName"] = groupName;
            var request = JsonConvert.SerializeObject(new { type = "SendMessageToGroup", uuid = Util.Config.userUuid, data = dataJson });
            Debug.Log(request);
            connection.InvokeAsync("SendMessageToGroup", request.ToString());
        }

        public void AddToGroup(string groupName="main")
        {
            var temp = new {groupName=groupName};
            var data = JsonConvert.SerializeObject(new { type = "AddToGroup", uuid = Util.Config.userUuid, data = temp });
            connection.InvokeAsync("AddToGroup", data);
        }

        public void RemoveFromGroup(string groupName="main")
        {
            var temp = new {groupName=groupName};
            var data = JsonConvert.SerializeObject(new { type = "RemoveFromGroup", uuid = Util.Config.userUuid, data = temp });
            connection.InvokeAsync("RemoveFromGroup", data);
        }
    }
}