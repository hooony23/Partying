using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Partying.Assets.Scripts.MainMenu
{
    public class ChatModule
    {

        public void Start()
        {
            var connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:1215/api/v1/rooms")
                    .WithAutomaticReconnect()
                    .Build();

            connection.On<string>("receive", data =>
            {
                Console.WriteLine($"receive: {data}");
            });

            connection.StartAsync();
        }
    }
}