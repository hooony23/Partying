using System;
using System.Threading;
namespace signalR_Test_Client
{
    class Program
    {
        static void Main(string[] argsd)
        {
            ChatModule chatModule = new ChatModule();
            chatModule.Start();
            while(true)
            {
                Console.WriteLine("****사용 가능 메소드****");
                Console.WriteLine("1. SendMessage");
                Console.WriteLine("2. SendMessageToGroup");
                Console.WriteLine("3. AddToGroup");
                Console.WriteLine("4. RemoveFromGroup");

                int input = int.Parse(Console.ReadLine());

                switch(input){
                    case 1:
                        Console.WriteLine("sendMessage 선택 확인");
                        Console.WriteLine("message 입력 대기");
                        string sendMessage = Console.ReadLine();
                        chatModule.SendMessage(sendMessage);
                        break;
                    case 2:
                        Console.WriteLine("sendMessage 선택 확인");
                        Console.WriteLine("groupName 입력 대기");
                        string sendGroupName = Console.ReadLine();
                        Console.WriteLine("message 입력 대기");
                        string sendGroupMessage = Console.ReadLine();
                        chatModule.SendMessageToGroup(sendGroupName, sendGroupMessage);                        
                        break;
                    case 3:
                        Console.WriteLine("AddToGroup 선택 확인");
                        Console.WriteLine("groupName 입력 대기");
                        string joinGroupName = Console.ReadLine();
                        chatModule.AddToGroup(joinGroupName);                        
                        break;
                    case 4:
                        Console.WriteLine("RemoveFromGroup 선택 확인");
                        Console.WriteLine("groupName 입력 대기");
                        string removeGroupName = Console.ReadLine();
                        chatModule.RemoveFromGroup(removeGroupName);                        
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
                Thread.Sleep(50);
            }
        }
    }
}