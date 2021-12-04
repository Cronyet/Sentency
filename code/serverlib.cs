using System;
using System.IO;

namespace Server{
    public class io{
        public static string[] infos = {
            "Sentency - A worldwide elegant website.",
            "Copyright Catrol .stu 2021",
            "Version: dev-1.0 - Update: 2021-11-22"
        };
        
        public static Random random = new Random();

        public static void log(string content){
            AppendTo($"{Environment.CurrentDirectory}\\log\\server-{random.Next(0,100)}.log", content);
        }
        
        public static void Help(){
            string helpDoc = $"{Environment.CurrentDirectory}\\help.txt";
            if(File.Exists(helpDoc)){
                FileStream fs = new FileStream(helpDoc, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                Console.WriteLine(sr.ReadToEnd());
                sr.Close(); sr.Dispose();
                fs.Close(); fs.Dispose();
            }else{
                Console.WriteLine("Help doc didn't found, please check it !");
            }
        }

        public static void WriteIn(string path, string content)
        {
            if(File.Exists(path)) { File.Delete(path); }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sr = new StreamWriter(fs);
            sr.Write(content);
            sr.Close(); sr.Dispose();
            fs.Close(); fs.Dispose();
        }
        
        public static void AppendTo(string path, string content)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sr = new StreamWriter(fs);
            sr.Write(content);
            sr.Close(); sr.Dispose();
            fs.Close(); fs.Dispose();
        }

        public static void init(){
            Console.WriteLine("Server.Helper class works well !");
        }

        public static void print(string text, ConsoleColor cf = ConsoleColor.White){
            ConsoleColor now = Console.ForegroundColor;
            Console.ForegroundColor = cf;
            Console.WriteLine(text);
            Console.ForegroundColor = now;
        }

        public static void seprate(){
            string output = "";
            for(int i = 0; i < 50; ++ i){ output += '-'; }
            print(output);
        }

        public static void read(out string tar) => tar = Console.ReadLine();

        public static void read(string tip, out string tar){
            Console.Write(tip);
            tar = Console.ReadLine();
        }

        public static void read(string tip, out string tar, ConsoleColor cf = ConsoleColor.White){
            ConsoleColor now = Console.ForegroundColor;
            Console.ForegroundColor = cf;
            Console.Write(tip);
            Console.ForegroundColor = now;
            tar = Console.ReadLine();
        }
        
        public static string read() => Console.ReadLine();
        
        public static string read(string tip){
            Console.Write(tip);
            return Console.ReadLine();
        }
        
        public static string read(string tip, ConsoleColor cf = ConsoleColor.White){
            ConsoleColor now = Console.ForegroundColor;
            Console.ForegroundColor = cf;
            Console.Write(tip);
            Console.ForegroundColor = now;
            return Console.ReadLine();
        }
    }
}