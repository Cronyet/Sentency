using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Server;

namespace Sentency{
    public class cmd{
        private static string ce = Server.MD5Helper.EncryptString("Sentency").ToUpper();

        public static Dictionary<string, XmlDocument> a = new Dictionary<string, XmlDocument>(),
        b = new Dictionary<string, XmlDocument>(), m = new Dictionary<string, XmlDocument>();

        public static int NativeCMD(string cmd){
            switch(cmd){
                case "exit": return 0; // 退出
                case "ls": return 1; // 打印客户端列表
                case "init": return 2; // 初始化目录地址
                case "path": return 3; // 打印目录地址
                case "help": return 4; // 打印帮助列表
                case "deport": return 5; // 驱逐客户端
                case "load": return 7; // 加载名言
                case "view": return 8; // 查看名言
                case "-a": return 9; // 查看作者分类
                case "-b": return 10; // 查看书籍分类
                case "-m": return 11; // 查看电影分类
                case "push": return 12; // 推送消息
                default:
                    if(cmd.Trim().ToUpper().Equals(ce)) return 6;
                    break;
            }
            return -1;
        }

        public static void initSentency(string path){
            if(File.Exists(path)){
                io.print("Analysing ...");
                XmlDocument doc = new XmlDocument(); // content.xml
                doc.Load(path);
                XmlElement root = doc.DocumentElement; // root in content.xml
                io.print($"\nRoot Node & It's ChildNodes:"); // root - inner xml
                io.print($"\n{root.InnerXml.ToString().Replace("/><", "/>\n<")}\n", ConsoleColor.Yellow);
                List<string> paths = new List<string>(); // other content xml path
                foreach(XmlNode node in root.ChildNodes){
                    string tpath = node.Attributes["content"].InnerText,
                    apath = $"{Path.GetDirectoryName(path)}{tpath.Substring(1, tpath.Length - 1).Replace('/', '\\')}";
                    io.print($"\t{tpath} \t-\t {apath}");
                    paths.Add(apath);
                }
                
                XmlDocument authors = new XmlDocument(), books = new XmlDocument(),
                movies = new XmlDocument(), unknown = new XmlDocument();
                XmlDocument[] doclist = {authors, books, movies, unknown};
                XmlElement[] roots = new XmlElement[4];
                int index = 0;

                io.print("");
                
                foreach(string loadPath in paths){
                    doclist[index].Load(loadPath);
                    roots[index] = doclist[index].DocumentElement;
                    io.print(roots[index].InnerXml.ToString().Replace("/><", "/>\n<"), ConsoleColor.Yellow);
                    foreach(XmlNode node in roots[index].ChildNodes){
                        var tmp = new XmlDocument();
                        string parentPath = new DirectoryInfo(Path.GetDirectoryName(path)).Parent.FullName;
                        switch (index){
                            case 0:
                                tmp.Load(parentPath + $"\\authors\\{node.Attributes["id"].InnerText}.xml");
                                a.Add(node.Attributes["name"].InnerText, tmp);
                                break;
                            case 1:
                                tmp.Load(parentPath + $"\\books\\{node.Attributes["id"].InnerText}.xml");
                                b.Add(node.Attributes["name"].InnerText, tmp);
                                break;
                            case 2:
                                tmp.Load(parentPath + $"\\movies\\{node.Attributes["id"].InnerText}.xml");
                                m.Add(node.Attributes["name"].InnerText, tmp);
                                break;
                        }
                    }
                    index ++;
                }
                io.print("\nAll sentency loaded !", ConsoleColor.Cyan);
            }else{ io.print("No this file ......"); }
        }

        public static string CommandProcess(string cmd) => cmd;
    }
}