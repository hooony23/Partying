using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
public class ChatModule : MonoBehaviour
{

    public void Start()
    {
        var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:1215/rooms")
                .WithAutomaticReconnect()
                .Build();

        connection.On<string>("receive", data =>
        {
            Console.WriteLine($"receive: {data}");
        });

        connection.On<string>("roomList", data =>
        {
            Console.WriteLine($"receive: {data}");
        });

        connection.On<string>("memberInfo", data =>
        {
            Console.WriteLine($"receive: {data}");
        });

        connection.StartAsync();
    }
}