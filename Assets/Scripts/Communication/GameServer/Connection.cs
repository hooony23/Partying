using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Communication.GameServer.API;
using Util;

namespace Communication.GameServer
{

    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024 * 8;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class Connection
    {
        // The port number for the remote device.  

        // AutoResetEvent instances signal completion.  
        public static AutoResetEvent connectDone =
            new AutoResetEvent(false);
        public static AutoResetEvent sendDone =
            new AutoResetEvent(false);
        public static AutoResetEvent receiveDone =
            new AutoResetEvent(false);

        // The response from the remote device.  
        private static String response = String.Empty;
        private static Socket client;
        public static void Connected(string request)
        {
            // Connect to a remote device.  
            try
            {
                // TODO Test끝나면 주석 해제
<<<<<<< HEAD:Assets/Scripts/Communication/GameServer/Connection.cs
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Config.defaultDNS);
                //  IPAddress ipAddress = ipHostInfo.AddressList[0];
                //  IPEndPoint remoteEP = new IPEndPoint(ipAddress, Config.gameServerPort);
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1045);
=======
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Config.serverIP);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1045);
>>>>>>> origin/dev-SungyuHwang:Assets/Scripts/Communication/Connection.cs

                // Create a TCP/IP socket.  
                client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                // Send(client, "{'type':'connectedExit'}<EOF>");
                Send(request);
                sendDone.WaitOne();
                Receive(client);
                receiveDone.WaitOne();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ConnectedExit()
        {
            try
            {
                Send("{'type':'ConnectedExit'}");
                sendDone.WaitOne();
                // Release the socket.  
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }
        // -------------- Connect
        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                // Console.WriteLine("Socket connected to {0}",
                //     client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // -------------- Receive
        public static void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;
                state.sb.Clear();
                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                String content = String.Empty;
                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {

                    state.sb.Append(Encoding.UTF8.GetString(
                        state.buffer, 0, bytesRead));

                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.  
                        string[] receiveDatas = content.Split(new string[] { "<EOF>" }, StringSplitOptions.None);
                        List<string> tmp = new List<string>(receiveDatas);
                        tmp.Remove("");
                        receiveDatas = tmp.ToArray();
                        foreach (string data in receiveDatas)
                        {
<<<<<<< HEAD:Assets/Scripts/Communication/GameServer/Connection.cs
=======
                            if (data.Contains("connected"))
                            {
                                response = data;
                                continue;
                            }
>>>>>>> origin/dev-SungyuHwang:Assets/Scripts/Communication/Connection.cs
                            APIController.ReceiveController(data);
                        }
                        receiveDone.Set();
                        state.sb.Clear();
                    }
                }
                state.workSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        // -------------- Send
        public static void Send(String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.UTF8.GetBytes(data + "<EOF>");
            // Console.WriteLine("send {0}", data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        // public static int Main(String[] args)
        // {
        //     Connected();
        //     ConnectedExit();
        //     return 0;
        // }
    }
}