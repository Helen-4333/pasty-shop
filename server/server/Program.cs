using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Program
    {
        static List<Order> order = new List<Order>();

        static public Byte[] swither(string message)
        {
            string[] comand = message.Split(new char[] { '@' });
            string[] buy = comand[1].Split(new char[] { '#' });

            switch (comand[0])
            {
                case "add":
                    {
                        order.Add(new Order(buy[0], buy[1]));
                        return (Encoding.UTF8.GetBytes(order.Find(x => x.buyer == buy[0]).collect_order()));
                    }
                case "del":
                    {
                        order.Remove(order.Find(x => x.buyer == buy[0]));
                    }
                    break;
                case "edit":
                    {
                        order.Remove(order.Find(x => x.buyer == buy[0]));
                        order.Add(new Order(buy[0], buy[1]));
                        return (Encoding.UTF8.GetBytes(order.Find(x => x.buyer == buy[0]).collect_order()));    
                    }
                case "view":
                    {
                        return (Encoding.UTF8.GetBytes(order.Find(x => x.buyer == buy[0]).collect_order()));
                    }
                case "view_user":
                    {
                        string str = "";
                        foreach (var item in order)
                        {
                            str += $"{item.buyer}#";
                        }
                        return (Encoding.UTF8.GetBytes(str));
                    }
                case "push":
                    {
                        order.Remove(order.Find(x => x.buyer == buy[0]));
                    }
                    break;
                case "check":
                    {
                        if (order.Find(x => x.buyer == buy[0]) == null)
                            return (Encoding.UTF8.GetBytes("no"));
                        return (Encoding.UTF8.GetBytes("yes"));
                    }
                case "viewall":
                    {
                        string str = "";

                        foreach (var item in order)
                        {
                            str += $"{item.collect_order()}@";
                        }
                        return (Encoding.UTF8.GetBytes(str));
                    }
                default:
                    break;
            }
            return (Encoding.UTF8.GetBytes(""));
        }

        static void Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener server = new TcpListener(localAddr, port);
            server.Start(); 
            Console.WriteLine("Сервер работает!");

            while (true)
            {
                try
                {
                    // Подключение клиента
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    // Обмен данными
                    try
                    {
                        if (stream.CanRead)
                        {
                            byte[] myReadBuffer = new byte[1024];
                            StringBuilder myCompleteMessage = new StringBuilder();
                            int numberOfBytesRead = 0;
                            do
                            {
                                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                                myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(myReadBuffer, 0, numberOfBytesRead));
                            }
                            while (stream.DataAvailable);
                            Byte[] responseData = swither(myCompleteMessage.ToString());
                            stream.Write(responseData, 0, responseData.Length);
                        }
                    }
                    finally
                    {
                        stream.Close();
                        client.Close();
                    }
                }
                catch
                {
                    server.Stop();
                    break;
                }
            }
        }
    }
}
