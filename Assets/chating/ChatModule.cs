using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json.Linq;
using signalR_Test_Client;
using Partying.UI;

namespace signalR_Test_Client
{
    public class ChatModule
    {
        ChattingUi aaa = new ChattingUi();
        HubConnection connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/chat")
            // .WithAutomaticReconnect()
            .Build();
        public void Start()
        {//웹 소켓 방식, 항상 연결이 아닌, 서버가 ReceiveMessage 메세지를 보내면 연결
            connection.On<string>("ReceiveMessage", data =>
            {
                aaa.SendMessageToChat(data);
            });

            connection.StartAsync();
        }

        public void SendMessage(string message)
        {
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("message",message);

            JB.Add("type","SendMessage");
            JB.Add("uuid",connection.ConnectionId);
            JB.Add("data",data);
            string JBST = JB.ToString();
            connection.InvokeAsync("SendMessage", JBST);
            Console.WriteLine($"###SendMessage complete### message: {message}");
        }

        public void SendMessageToGroup(string groupName, string message)
        {
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("groupName",groupName);
            data.Add("message",message);

            JB.Add("type","SendMessageToGroup");
            JB.Add("uuid",connection.ConnectionId);
            JB.Add("data",data);
            string JBST = JB.ToString();
            connection.InvokeAsync("SendMessageToGroup", JBST);
            Console.WriteLine($"###SendMessageToGroup complete### groupName: {groupName} / message: {message}");
        }

        public void AddToGroup(string groupName)
        {
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("groupName",groupName);

            JB.Add("type","AddToGroup");
            JB.Add("uuid",connection.ConnectionId);
            JB.Add("data",data);
            string JBST = JB.ToString();
            connection.InvokeAsync("AddToGroup",JBST);
            Console.WriteLine($"###AddToGroup complete### groupName: {groupName}");
        }

        public void RemoveFromGroup(string groupName)
        {            
            JObject JB = new JObject();
            JObject data = new JObject();

            data.Add("groupName",groupName);

            JB.Add("type","RemoveFromGroup");
            JB.Add("uuid",connection.ConnectionId);
            JB.Add("data",data);
            string JBST = JB.ToString();
            connection.InvokeAsync("RemoveFromGroup",JBST);
            Console.WriteLine($"###RemoveFromGroup complete### groupName: {groupName}");
        }
    }
}