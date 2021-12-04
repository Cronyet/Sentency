using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Sentency;
using System.Xml;

namespace Server{
    public class Entry{
        
        private static TcpListener _listener;

        private static bool _isAccept = true;

        private static Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>();

        private static string contentxml_path = "";

        public static Random rand = new Random();

        public static void Main(string[] args){
            foreach(string key in io.infos) io.print(key, ConsoleColor.Cyan);
            io.seprate();
            Thread acceptClientThread = new Thread(AcceptClient);
            if(io.read("Ready to start services ? (y/n): ", ConsoleColor.Blue).Equals("y")){
                try{
                    _listener = new TcpListener(IPAddress.Any, 10800);
                    _listener.Start();
                }catch{
                    io.print("TcpListener starting failed !", ConsoleColor.Red);
                }
                
                acceptClientThread.Start();
            }else{ return; }
            while(_isAccept){
                string cmd_in = io.read("sentency # ", ConsoleColor.Magenta).Trim().Replace("  ", " ");
                string[] cs = cmd_in.Split(' ');
                switch(cmd.NativeCMD(cs[0].Trim())){
                    case 0:
                        _isAccept = false;
                        foreach(var c in _clients.Values){
                            c.Close();
                        }
                        _listener.Stop();
                        io.print("Server shutdown !", ConsoleColor.Green);
                        break;
                    case 1:
                        if(_clients.Count > 0){
                            foreach(var c in _clients.Values){
                                IPEndPoint endpoint = c.Client.RemoteEndPoint as IPEndPoint;
                                io.print(endpoint.ToString(), ConsoleColor.Gray);
                            }
                        }else{
                            io.print("No client connecting !", ConsoleColor.Yellow);
                        }
                        break;
                    case 2:
                        if(cs.Length > 1){
                            contentxml_path = cs[1].Trim();
                        }else{
                            contentxml_path = io.read("Input 'content.xml' path : ", ConsoleColor.Blue);
                        }
                        if(!File.Exists(contentxml_path)) io.print("File path doesn't exists !", ConsoleColor.Red);
                        break;
                    case 3:
                        bool exist = File.Exists(contentxml_path);
                        io.print(contentxml_path, exist ? ConsoleColor.Cyan : ConsoleColor.Red);
                        if(!exist) io.print("File doesn't exist !", ConsoleColor.Red);
                        break;
                    case 4:
                        io.Help();
                        break;
                    case 5:
                        string ip;
                        if(cs.Length > 1){
                            ip = cs[1].Trim();
                        }else{
                            ip = io.read("Input target IP : ", ConsoleColor.Yellow).Trim();
                        }
                        foreach(var c in _clients.Values){
                            IPEndPoint endpoint = c.Client.RemoteEndPoint as IPEndPoint;
                            if(ip.ToUpper().Equals("ALL") || endpoint.ToString().Equals(ip)) c.Close();
                        }
                        break;
                    case 6: // 彩蛋
                        io.print("Welcome to Catrol's world !", ConsoleColor.Cyan);
                        break;
                    case 7:
                        if(File.Exists(contentxml_path))
                            cmd.initSentency(contentxml_path);
                        else
                            io.print("Didn't init the content xml path !", ConsoleColor.Red);
                        break;
                    case 8:
                        if(cs.Length > 1){
                            switch(cmd.NativeCMD(cs[1].Trim())){
                                case 9:
                                    foreach(string authors_names in cmd.a.Keys){
                                        io.print($"\n@{authors_names}:", ConsoleColor.Green);
                                        foreach(XmlNode sen in cmd.a[authors_names].DocumentElement.ChildNodes){
                                            io.print($"\t{sen.Attributes["sentency"].InnerText}");
                                        }
                                    }
                                    break;
                                case 10:
                                    foreach(string books_names in cmd.b.Keys){
                                        io.print($"\n@{books_names}:", ConsoleColor.Green);
                                        foreach(XmlNode sen in cmd.b[books_names].DocumentElement.ChildNodes){
                                            io.print($"\t{sen.Attributes["sentency"].InnerText}");
                                        }
                                    }
                                    break;
                                case 11:
                                    foreach(string movies_names in cmd.m.Keys){
                                        io.print($"\n@{movies_names}:", ConsoleColor.Green);
                                        foreach(XmlNode sen in cmd.m[movies_names].DocumentElement.ChildNodes){
                                            io.print($"\t{sen.Attributes["sentency"].InnerText}");
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case 12:
                        string ct = "";
                        if(cs.Length > 1){
                            for(int i = 1; i < cs.Length; ++ i)
                                ct += cs[i] + ' ';
                        }else{
                            ct = io.read("Type the message that you want to push : ", ConsoleColor.Cyan);
                        }
                        foreach (KeyValuePair<string, TcpClient> kvp in _clients){
                            byte[] writeData = Encoding.UTF8.GetBytes(ct);
                            NetworkStream writeStream = kvp.Value.GetStream();
                            writeStream.Write(writeData, 0, writeData.Length);
                        }
                        break;
                    default:
                        if(cmd_in.Trim().Length > 0)
                            io.print($"Command '{cmd_in.Trim()}' is not recognized as a receivable command !", ConsoleColor.Red);
                        break;

                }
            }
            io.print("Press any Key to exit ...", ConsoleColor.Yellow);
            io.read();
        }

        private static void AcceptClient(){
            try{
                while (_isAccept){
                    if (_listener.Pending()){
                        TcpClient client = _listener.AcceptTcpClient();
                        IPEndPoint endpoint = client.Client.RemoteEndPoint as IPEndPoint;
                        _clients.Add(endpoint.ToString(), client);
                        
                        Thread reciveMessageThread = new Thread(ReciveMessage);
                        reciveMessageThread.Start(client);

                        new Thread(() => {
                            Dictionary<string, XmlDocument> ca = new Dictionary<string, XmlDocument>(cmd.a);
                            Dictionary<string, XmlDocument> cb = new Dictionary<string, XmlDocument>(cmd.b);
                            Dictionary<string, XmlDocument> cm = new Dictionary<string, XmlDocument>(cmd.m);
                            while(_isAccept){
                                Thread.Sleep(1000);
                                int fl = rand.Next(1, 4);
                                Dictionary<string, XmlDocument> fd = (fl == 1 ? ca : (fl == 2 ? cb : cm));
                                int a = rand.Next(1, (fl == 1 ? cmd.a : (fl == 2 ? cmd.b : cmd.m)).Count + 1), counter = 0;
                                string content = "";
                                foreach(string authors_names in fd.Keys){
                                    counter ++;
                                    if(counter == a){
                                        int b = rand.Next(1, fd[authors_names].DocumentElement.ChildNodes.Count + 1), c2 = 0;
                                        foreach(XmlNode sen in fd[authors_names].DocumentElement.ChildNodes){
                                            c2 ++;
                                            if(c2 == b){
                                                content = $"{sen.Attributes["sentency"].InnerText}\n\t{authors_names}";
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                foreach (KeyValuePair<string, TcpClient> kvp in _clients){
                                    if (kvp.Value == client){
                                        byte[] writeData = Encoding.UTF8.GetBytes(content);
                                        NetworkStream writeStream = kvp.Value.GetStream();
                                        writeStream.Write(writeData, 0, writeData.Length);
                                    }
                                }
                            }
                        }).Start();
                    }else{ Thread.Sleep(1000); }
                }
            }
            catch (Exception ex){ Console.WriteLine(ex.Message); }
        }

        private static void ReciveMessage(object obj){
            TcpClient client = obj as TcpClient;
            IPEndPoint endpoint = null;
            NetworkStream stream = null;

            try{
                endpoint = client.Client.RemoteEndPoint as IPEndPoint;
                stream = client.GetStream();

                while (_isAccept){
                    byte[] data = new byte[1024];
                    int length = stream.Read(data, 0, data.Length);
                    if (length > 0){
                        string msg = Encoding.UTF8.GetString(data, 0, length);
                        Console.WriteLine(string.Format("{0}:{1}", endpoint.ToString(), msg));
                        string result = cmd.CommandProcess(msg);
                        foreach (KeyValuePair<string, TcpClient> kvp in _clients){
                            if (kvp.Value == client){
                                byte[] writeData = Encoding.UTF8.GetBytes(msg);
                                NetworkStream writeStream = kvp.Value.GetStream();
                                writeStream.Write(writeData, 0, writeData.Length);
                            }
                        }
                    }else{
                        io.print($"{endpoint.ToString()} 已断开连接", ConsoleColor.DarkCyan);
                        break;
                    }
                }
            }catch(Exception ex){ io.log(ex.Message); }
            finally{
                stream.Dispose();
                _clients.Remove(endpoint.ToString());
                client.Dispose();
            }
        }
    }
}