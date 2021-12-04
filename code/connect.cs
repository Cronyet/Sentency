using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Program{
    public class Test{

        private static TcpClient _client;

        public static void Main(string[] args){
            _client = new TcpClient();
            _client.Connect(IPAddress.Parse(Console.ReadLine()), 10800);

            new Thread(() => {
                try
                {
                    NetworkStream stream = _client.GetStream();
                    while (true)
                    {
                        byte[] data = new byte[1024];
                        int length = stream.Read(data, 0, data.Length);
                        if (length > 0)
                        {
                            string msg = Encoding.UTF8.GetString(data, 0, length);
                            Console.WriteLine($"收到：{msg}");
                        }
                        else
                        {
                            Console.WriteLine("连接已断开");
                            stream.Dispose();
                            break;
                        }
                    }
                }catch{}
            }).Start();
            
            while(true){
                string msg = Console.ReadLine();
                if(msg.Equals("exit")) break;
                byte[] data = Encoding.UTF8.GetBytes($"zty：{msg}");
                try
                {
                    NetworkStream stream = _client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"连接已断开\n{ex.Message}");
                    break;
                }
            }
            
            _client.Close();
            _client.Dispose();

            Console.ReadLine();
        }
    }
}