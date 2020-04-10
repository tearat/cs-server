using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace FirstServer
{
    class Response
    {
        Byte[] data = null;
        String status = null;
        String mime = null;
        private Response(String status, String mime, Byte[] data) 
        {
            this.data = data;
            this.status = status;
            this.mime = mime;
        }

        public static Response From(Request request)
        {
            if(request == null)
            {
                return NotWork("400.html", "400 Bad Request");
            }
            if(request.Type == "GET")
            {
                String file = Environment.CurrentDirectory + HttpServer.WEB_DIR + request.Url;
                FileInfo fi = new FileInfo(file);
                if(fi.Exists && fi.Extension.Contains('.'))
                {
                    return MakeFromFile(fi);
                } else
                {
                    DirectoryInfo di = new DirectoryInfo(fi + "/");
                    if(!di.Exists)
                    {
                        return NotWork("404.html", "404 Page Not Found");
                    }
                    FileInfo[] files = di.GetFiles();
                    foreach(FileInfo ff in files)
                    {
                        if(ff.Name.Contains("default.html") || ff.Name.Contains("index.html"))
                        {
                            return MakeFromFile(ff);
                        }
                    }
                }
            }
            else
            {
                return NotWork("405.html", "405 Method Not Allowed");
            }
            return NotWork("404.html", "404 Page Not Found");
            //String file = Environment.CurrentDirectory + HttpServer.WEB_DIR + request;
        }

        private static Response MakeFromFile(FileInfo fi)
        {
            FileStream fs = fi.OpenRead();
            Byte[] data = new Byte[fs.Length];
            BinaryReader reader = new BinaryReader(fs);
            reader.Read(data, 0, data.Length);

            return new Response("200 OK", "text/html", data);
        }

        private static Response NotWork(String fileName, String status)
        {
            String file = Environment.CurrentDirectory + HttpServer.MSG_DIR + fileName;
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] data = new Byte[fs.Length];
            reader.Read(data, 0, data.Length);

            return new Response(status, "text/html", data);
        }

        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            String response = String.Format("{0} {1}\r\nServer: {2}\r\nContent-Language: ru\r\nContent-type: {3}\r\nAccept-Ranges: bytes\r\nContentLength: {4}\r\nConnection: close\r\n", HttpServer.VERSION, status, HttpServer.SERVERNAME, mime, data.Length);
            Console.WriteLine(response);
            writer.WriteLine(response);
            writer.Flush();

            stream.Write(data, 0, data.Length);
        }
    }
}
