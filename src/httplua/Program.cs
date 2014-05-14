using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace httplua
{
    class Program
    {
        static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var ms = new System.IO.MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        static object ByteArrayToObject(byte[] arr)
        {
            if (arr == null)
                return null;

            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var ms = new System.IO.MemoryStream(arr);

            return bf.Deserialize(ms);
        }


        static void Main(string[] args)
        {
            string funcName = String.Empty;
            Lua lua = new Lua();

            if (args.Length == 3)
            {
                var httpRequest = new HttpRequest(args[2], (HttpRequestType)Enum.Parse(typeof(HttpRequestType), args[1], true));
                lua["http_request"] = httpRequest;
            }
            else if (args.Length == 4)
            {
                var httpRequest = new HttpRequest(args[2], (HttpRequestType)Enum.Parse(typeof(HttpRequestType), args[1], true));
                lua["http_request"] = httpRequest;
                funcName = args[1];
            }

            try
            {
                foreach (var file in System.IO.Directory.GetFiles("scripts"))
                {
                    lua.DoFile(file);
                }

                HttpLuaParser parser = HttpLuaParser.Parse(args[0]);
                lua.DoString(parser.LuaCode.ToString());
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() != null)
                {
                    Console.WriteLine(ex.GetBaseException().ToString());
                }
                else
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
