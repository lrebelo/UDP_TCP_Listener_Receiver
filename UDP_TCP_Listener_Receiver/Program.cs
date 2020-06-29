using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDP_TCP_Listener_Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 0)
            {
                Console.WriteLine("\n## Network Utility :: TCP-socket Server UDP-Broadcast Receiver ##\n## Luis Rebelo - 2017/2020 ##\n\n");

                Console.WriteLine("\n 1:: for TCP Socket server\n 2:: for UDP Receiver\n");
                string opt = Console.ReadLine();

                if (opt.Equals("1"))
                {
                    Console.WriteLine("\nPort:\n");
                    int port = Convert.ToInt32(""+Console.ReadLine());
                    TCPsocketServer tcpServer = new TCPsocketServer(port);
                }
                else if (opt.Equals("2"))
                {
                    Console.WriteLine("\nPort:\n");
                    int port = Convert.ToInt32("" + Console.ReadLine());
                    UDPbroadcastServer udpServ = new UDPbroadcastServer(port);
                }
            }
            
        }
    }

    class TCPsocketServer
    {
        public TCPsocketServer(int port)
        {

            try
            {

                TcpListener serverSocket = new TcpListener(port);
                TcpClient clientSocket = default(TcpClient);
                serverSocket.Start();
                Console.WriteLine("\n TCPsocketServer :: Started on port: {0}\n", port);
                clientSocket = serverSocket.AcceptTcpClient();

                NetworkStream networkStream = clientSocket.GetStream();
                byte[] brodReciveByt = new byte[(int)clientSocket.ReceiveBufferSize];
                networkStream.Read(brodReciveByt, 0, (int)clientSocket.ReceiveBufferSize);

                UTF8Encoding temp = new UTF8Encoding(true);
                string brodReciveString = temp.GetString(brodReciveByt);
                Console.WriteLine("TCPsocketServer :: Recived \n {0}", brodReciveString);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n TCPsocketServer :: Exception 1 ::: {0}\n", e.ToString());
            }

        }
    }

    class UDPbroadcastServer
    {
        public UDPbroadcastServer(int port)
        {
            try
            {

                UdpClient udpClient = new UdpClient();
                try
                {
                    Console.WriteLine("\n UDPbroadcastServer :: Listening for Broadcast on port: {0}\n", port);
                    udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
                    IPEndPoint brdcatLoc = new IPEndPoint(0, 0);
                    while (true)
                    {
                        byte[] brodReciveByt = udpClient.Receive(ref brdcatLoc);
                        string brodReciveString = Encoding.ASCII.GetString(brodReciveByt);
                        Console.WriteLine("UDPbroadcastServer :: Recived: \n {0} \n from {1}\n\n", brodReciveString, brdcatLoc.Address.ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n UDPbroadcastServer :: Exception 1 ::: {0}\n", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\n UDPbroadcastServer :: Exception 2 ::: {0}\n", e.ToString());
            }

        }
    }
}
