using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;

namespace httplua.DB
{
    class SQLite
    {
        string connectionString = String.Empty;
        SQLiteConnection connection = null;

        public SQLite(string dataSource, string password)
        {
            this.connectionString =
                String.Format("data source={0};password={1};", dataSource, password);
            this.connection = new SQLiteConnection(connectionString);
            this.connection.Open();
        }

        public void Close()
        {
            this.connection.Close();
        }

        public SQLiteDataReader Query(string command)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(command, this.connection))
            {
                return cmd.ExecuteReader();
            }
        }

        public SQLiteDataReader Query(SQLiteCommand cmd)
        {
            return cmd.ExecuteReader();
        }

        public List<Dictionary<string, string>> FetchAssoc(SQLiteDataReader reader)
        {
            List<Dictionary<string, string>> assocValues = new List<Dictionary<string, string>>();

            List<string> columns = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }

            while (reader.Read())
            {
                Dictionary<string, string> value = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    value.Add(columns[i], reader[i].ToString());
                }
                assocValues.Add(value);
            }
            reader.Close();
            return assocValues;
        }

        public List<string> FetchArray(SQLiteDataReader reader)
        {
            List<string> arrayValues = new List<string>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    arrayValues.Add(reader[i].ToString());
                }
            }
            reader.Close();
            return arrayValues;
        }

        public SQLiteCommand Prepare(string command, NLua.LuaTable table)
        {
            SQLiteCommand cmd = new SQLiteCommand(command, this.connection);
            foreach (System.Collections.DictionaryEntry param in table)
            {
                //if (param.Value.GetType() == typeof(LuaInterface.LuaTable))
                //{
                //    var objConverter = System.ComponentModel.TypeDescriptor.GetConverter(param.Value.GetType());
                //    byte[] data = (byte[])objConverter.ConvertTo(param.Value, typeof(byte[]));
                //}
                cmd.Parameters.Add(new SQLiteParameter(param.Key.ToString(), param.Value.ToString()));
            }
            return cmd;
        }

        public byte[] GetBytes(SQLiteDataReader reader)
        {
            if (reader.Read())
            {
                const int CHUNK_SIZE = 2 * 1024;
                byte[] buffer = new byte[CHUNK_SIZE];
                long bytesRead;
                long fieldOffset = 0;
                using (MemoryStream stream = new MemoryStream())
                {
                    while ((bytesRead = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, (int)bytesRead);
                        fieldOffset += bytesRead;
                    }
                    return stream.ToArray();
                }
            }
            return null;
        }

        public SQLiteConnection Connection { get { return this.connection; } }
    }
}
