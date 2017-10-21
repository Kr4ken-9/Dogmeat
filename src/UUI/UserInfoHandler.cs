using System;
using System.IO;
using System.Threading.Tasks;
using Dogmeat.Config;
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
            Connection = new MySqlConnection($"SERVER={CString.Server};" +
                                             $"DATABASE={CString.Database};" +
                                             $"UID={CString.UID};" +
                                             $"PASSWORD={CString.Password};" +
                                             $"PORT={CString.Port};");

            ConnectionString = CString;
            
            CheckSchema();
        }

        internal async Task CheckSchema()
        {
            try
            {
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "SHOW TABLES LIKE 'Dogmeat'";
            
                await Connection.OpenAsync();

                if (await Command.ExecuteScalarAsync() != null)
                    return;

                Command.CommandText =
                    "CREATE TABLE Dogmeat" +
                    "(ID varchar(32) NOT NULL, " +
                    "Experience MEDIUMINT(8) UNSIGNED NOT NULL, " +
                    "PRIMARY KEY (ID))";

                await Command.ExecuteNonQueryAsync();
                Connection.Close();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public void SaveConnection() =>
            File.WriteAllText(ConfigManager.ConfigPath("mysql.json"),
                JsonConvert.SerializeObject(ConnectionString, Formatting.Indented));

        public static UserInfoHandler LoadConnection()
        {
            Connection C = JsonConvert.DeserializeObject<Connection>
                (File.ReadAllText(ConfigManager.ConfigPath("mysql.json")));

            return new UserInfoHandler(C);
        }

        public static Connection AggregateConnection(String Input)
        {
            switch (Input.ToUpperInvariant())
            {
                case "YES":
                case "Y":
                    String Server = "";
                    String Database = "";
                    String UID = "";
                    String Password = "";
                    int Port;

                    Console.WriteLine("Enter server address:");
                    Server = Console.ReadLine();
                        
                    Console.WriteLine("Enter database name:");
                    Database = Console.ReadLine();
                        
                    Console.WriteLine("Enter user name:");
                    UID = Console.ReadLine();
                        
                    Console.WriteLine("Enter password:");
                    Password = Console.ReadLine();
                        
                    Console.WriteLine("Enter port:");
                    Port = int.Parse(Console.ReadLine());
                    
                    return new Connection(Server, Database, UID, Password, Port);
                case "NO":
                case "N":
                    return null;
                default:
                    Console.WriteLine("Defaulting to no.");
                    return null;
            }
        }
    }
}