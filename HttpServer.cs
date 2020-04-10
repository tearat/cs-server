using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FirstServer
{
    class HttpServer
    {
        public const String VERSION = "Http/1.0";
        public const String SERVERNAME = "myserver/1.1";
        public const String MSG_DIR = "/root/msg/";
        public const String WEB_DIR = "/root/web/";
        TcpListener listener;
        bool running = false;

        public HttpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }

        private void Run()
        {
            listener.Start();
            running = true;

            Console.WriteLine("Server is running");

            while(running)
            {
                Console.WriteLine("Waiting fro connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                HandleClient(client);
                client.Close();
            }
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            String msg = "";
            while(reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\n";
            }

            Console.WriteLine("REQUEST: \n\n {0}", msg);

            Request request = Request.GetRequest(msg);
            Response response = Response.From(request);
            response.Post(client.GetStream());
        }
    }
}
