using System;
using System.IO;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Dogmeat.UUI
{
    public class UserInfoHandler
    {
        public MySqlConnection Connection;

        public Connection ConnectionString;
        
        public UserInfoHandler(Connection CString)
        {
            Connection =
                new MySqlConnection(
                    $"SERVER={CString.Server};DATABASE={CString.Database};UID={CString.UID};PASSWORD={CString.Password};PORT={CString.Port};");

            ConnectionString = CString;
            
            CheckSchema();
        }

        internal async Task CheckSchema()
        {
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = "show tables like `users`";
            
            await Connection.OpenAsync();

            if (await Command.ExecuteScalarAsync() != null)
                return;

            Command.CommandText =
                "CREATE TABLE `users` (`ID` varchar(32) NOT NULL, `Experience` MEDIUMINT(8) UNSIGNED NOT NULL, PRIMARY KEY (`ID`))";

            await Command.ExecuteNonQueryAsync();
            Connection.Close();
        }

        public async Task SaveConnection() => File.WriteAllText("config//mysql.json",
            JsonConvert.SerializeObject(ConnectionString, Formatting.Indented));
    }
}