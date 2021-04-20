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

namespace signalR_Test_Client
{
    public class ChatModule : MonoBehaviour
    {/*
        HubConnection connection = new HubConnectionBuilder()
            .WithUrl($"{Config.chatServerDNS}/chat")
            // .WithAutomaticReconnect()
            .Build();*/
        HubConnection connection = new HubConnectionBuilder()
                    .WithUrl($"https://skine134.iptime.org:42460/chat").Build();
        // static으로 현재시간 변수 선언
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

        public void SendMessage(string message)
        {

            ChatInfo requestData = new ChatInfo();
            requestData.Nickname = "asdf";//myData.NickName;
            requestData.Message = message;
            //var request = JObject.FromObject(requestData);
            var request = JsonConvert.SerializeObject(new { type = "SendMessage", uuid = Util.Config.userUuid, data = requestData });
            Debug.Log(request);
            connection.InvokeAsync("SendMessage", request.ToString());
        }
        /*BaseJsonFormat.ObjectToJson("signUp",requestData);
            connection.InvokeAsync("SendMessage", request.ToString());
            //Console.WriteLine($"###SendMessage complete### message: {message}");*/
        

        public void SendMessageToGroup(string groupName, string message)
        {
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("groupName", groupName);
            data.Add("message", message);
            data.Add("time", currentTime);
            data.Add("nickname", "nickname");

            JB.Add("type", "SendMessageToGroup");
            JB.Add("uuid", "uuid");
            JB.Add("data", data);
            string JBST = JB.ToString();
            connection.InvokeAsync("SendMessageToGroup", JBST);
            Console.WriteLine($"###SendMessageToGroup complete### groupName: {groupName} / message: {message}");
        }

        public void AddToGroup(string groupName)
        {
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("groupName", groupName);
            data.Add("time", currentTime);
            data.Add("nickname", "nickname");

            JB.Add("type", "AddToGroup");
            JB.Add("uuid", "uuid");
            JB.Add("data", data);
            string JBST = JB.ToString();
            connection.InvokeAsync("AddToGroup", JBST);
            Console.WriteLine($"###AddToGroup complete### groupName: {groupName}");
        }

        public void RemoveFromGroup(string groupName)
        {
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("groupName", groupName);
            data.Add("time", currentTime);
            data.Add("nickname", "nickname");

            JB.Add("type", "RemoveFromGroup");
            JB.Add("uuid", "uuid");
            JB.Add("data", data);
            string JBST = JB.ToString();
            connection.InvokeAsync("RemoveFromGroup", JBST);
            Console.WriteLine($"###RemoveFromGroup complete### groupName: {groupName}");
        }
    }
}