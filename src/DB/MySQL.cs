using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace httplua.DB
{
    class MySQL
    {
        string connectionString = String.Empty;
        MySqlConnection connection = null;

        public MySQL(string host, string username, string password, string database, int timeout)
        {
            this.connectionString = 
                String.Format("server={0};uid={1};pwd={2};database={3};connection timeout={4};", host, username, password, database, timeout);
            this.connection = new MySqlConnection(connectionString);
            this.connection.Open();
        }

        public void Close()
        {
            this.connection.Close();
        }

        public MySqlCommand Prepare(string command, NLua.LuaTable table)
        {
            MySqlCommand cmd = new MySqlCommand(command, this.connection);
            foreach (System.Collections.DictionaryEntry param in table)
            {
                cmd.Parameters.Add(new MySqlParameter(param.Key.ToString(), param.Value.ToString()));
            }
            return cmd;
        }

        public MySqlDataReader Query(string command)
        {
            using (MySqlCommand cmd = new MySqlCommand(command, this.connection))
            {
                return cmd.ExecuteReader();
            }
        }

        public MySqlDataReader Query(MySqlCommand cmd)
        {
            return cmd.ExecuteReader();
        }

        public List<Dictionary<string, string>> FetchAssoc(MySqlDataReader reader)
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

        public List<string> FetchArray(MySqlDataReader reader)
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
    }
}
