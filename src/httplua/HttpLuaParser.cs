using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace httplua
{
    class HttpLuaParser
    {
        private HttpLuaParser(StringBuilder parsedLua)
        {
            this.LuaCode = parsedLua;
        }

        public static HttpLuaParser Parse(string file)
        {
            StringBuilder lua = new StringBuilder();
            bool inCode = false;

            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith("<httplua"))
                    {
                        inCode = true;
                        continue;
                    }
                    else if (line.StartsWith("httplua>"))
                    {
                        inCode = false;
                        continue;
                    }

                    if (inCode)
                    {
                        lua.AppendLine(line);
                    }
                    else
                    {
                        lua.AppendLine("print [[");
                        lua.AppendLine(line);
                        while (!reader.EndOfStream)
                        {
                            string textLine = reader.ReadLine();
                            if (textLine.Contains("<httplua"))
                            {
                                inCode = true;
                                break;
                            }
                            lua.AppendLine(textLine);
                        }
                        lua.AppendLine("]]");
                    }
                }
            }

            return new HttpLuaParser(lua);
        }

        public StringBuilder LuaCode { get; private set; }
    }
}
