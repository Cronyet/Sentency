using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Program{
    public class Repair{
        public static Dictionary<string, XmlDocument> a = new Dictionary<string, XmlDocument>(),
        b = new Dictionary<string, XmlDocument>(), m = new Dictionary<string, XmlDocument>();

        public static void Main(string[] args){
            Console.WriteLine("Program running ...");
            Console.Write("Input XML (content.xml) File Path : ");
            string path = Console.ReadLine();
            // C:\Users\23699\OneDrive\Development\Projects\Sentency\src\content\content.xml
            if(File.Exists(path)){
                Console.WriteLine("Analysing ...");
                XmlDocument doc = new XmlDocument(); // content.xml
                doc.Load(path);
                XmlElement root = doc.DocumentElement; // root in content.xml
                Console.WriteLine($"\nRoot Node & It's ChildNodes:"); // root - inner xml
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\n{root.InnerXml}\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("foreaching...");
                List<string> paths = new List<string>(); // other content xml path
                foreach(XmlNode node in root.ChildNodes){
                    string tpath = node.Attributes["content"].InnerText,
                    apath = $"{Path.GetDirectoryName(path)}{tpath.Substring(1, tpath.Length - 1).Replace('/', '\\')}";
                    Console.WriteLine($"\t{tpath} \t-\t {apath}");
                    paths.Add(apath);
                }

                Console.WriteLine("Loading every one ...\n");
                XmlDocument authors = new XmlDocument(), books = new XmlDocument(),
                movies = new XmlDocument(), unknown = new XmlDocument();
                XmlDocument[] doclist = {authors, books, movies, unknown};
                XmlElement[] roots = new XmlElement[4];
                int index = 0;
                
                foreach(string loadPath in paths){
                    doclist[index].Load(loadPath);
                    roots[index] = doclist[index].DocumentElement;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(roots[index].InnerXml);
                    Console.ForegroundColor = ConsoleColor.White;
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
                Console.WriteLine("\nAll sentency loaded !");

                Console.Write("\nWould you like to view all authors with there sentencies ? (y/n) : ");
                if(Console.ReadLine() == "y"){
                    foreach(string authors_names in a.Keys){
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n@{authors_names}:");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        foreach(XmlNode sen in a[authors_names].DocumentElement.ChildNodes){
                            Console.WriteLine($"\t{sen.Attributes["sentency"].InnerText}");
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nWould you like to view all books with there sentencies ? (y/n) : ");
                if(Console.ReadLine() == "y"){
                    foreach(string books_names in b.Keys){
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n@{books_names}:");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        foreach(XmlNode sen in b[books_names].DocumentElement.ChildNodes){
                            Console.WriteLine($"\t{sen.Attributes["sentency"].InnerText}");
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nWould you like to view all movies with there sentencies ? (y/n) : ");
                if(Console.ReadLine() == "y"){
                    foreach(string movies_names in m.Keys){
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n@{movies_names}:");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        foreach(XmlNode sen in m[movies_names].DocumentElement.ChildNodes){
                            Console.WriteLine($"\t{sen.Attributes["sentency"].InnerText}");
                        }
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("\nAll down! Now enter Command Line Mode.\n\n");
                CLM();

            }else{ Console.WriteLine("No this file, program ended.\n\nPress any key to Exit ......"); }
            Console.ReadLine();
        }

        private static void xmcTip() => Console.Write("xmc | ");

        private static void CLM(){
            bool con = true;
            while(con){
                xmcTip();
                string cmd = Console.ReadLine();
                cmd = cmd.Trim();
                string ins = cmd.Trim().Substring(0, cmd.IndexOf(' ') > 0 ? cmd.IndexOf(' ') : cmd.Length).ToLower();
                switch(ins){
                    case "exit": con = false; break;
                    case "view":
                        
                        break;
                    default:
                        xmcTip();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"No this command named : '{ins}', please check your command.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }
    }
}