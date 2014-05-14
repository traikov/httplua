using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace httplua
{
    class HttpRequest
    {
        public HttpRequest(string args, HttpRequestType type)
        {
            Request = new Dictionary<string, string>();
            string[] getKeyValue = args.Split('&');
            foreach (var s in getKeyValue)
            {
                string[] x = s.Split('=');
                if (x.Length == 2)
                {
                    this.Request.Add(x[0], x[1]);
                }
            }
            RequestType = type;
        }

        public Dictionary<string, string> Request { get; private set; }

        public HttpRequestType RequestType { get; private set; }
    }

    public enum HttpRequestType
    {
        GET,
        POST
    }
}
