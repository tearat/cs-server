using System;

namespace FirstServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            HttpServer server = new HttpServer(8080);
            server.Start();
        }
    }
}
