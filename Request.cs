using System;
using System.Collections.Generic;
using System.Text;

namespace FirstServer
{
    class Request
    {
        public String Type { get; set; }
        public String Url { get; set; }
        public String Host { get; set; }
        private Request(String type, String url, String host)
        {
            Type = type;
            Url = url;
            Host = host;
        }

        public static Request GetRequest(String msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                return null;
            }
            String[] tokens = msg.Split(' ');
            Console.WriteLine("TYPE: {0}, URL: {1}, HOST: {2}", tokens[0], tokens[1], tokens[3]);
            return new Request(tokens[0], tokens[1], tokens[3]);
        }
    }
}
