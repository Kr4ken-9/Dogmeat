using System;
using System.IO;
using System.Threading.Tasks;
using Dogmeat.Config;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Dogmeat.Database
{
    public class DatabaseHandler
    {
        public MySqlConnection Connection;

        public Connection ConnectionString;

        public UserInfoHandler UUIHandler;

        public TagHandler TagHandler;
        
        public DatabaseHandler(Connection CString)
        {
            Connection = new MySqlConnection($"SERVER={CString.Server};" +
                                             $"DATABASE={CString.Database};" +
                                             $"UID={CString.UID};" +
                                             $"PASSWORD={CString.Password};" +
                                             $"PORT={CString.Port};");

            ConnectionString = CString;
            UUIHandler = new UserInfoHandler(Connection);
            TagHandler = new TagHandler(Connection);

            CheckTables().GetAwaiter().GetResult();
        }
        
        #region Tables

        internal async Task CheckTables()
        {
            await CheckUsersTable();
            await CheckTagsTable();
            await CheckInsignasTable();
        }

        private async Task CheckUsersTable()
        {
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SHOW TABLES LIKE 'Users'";
            
                await Connection.OpenAsync();

                if (await Command.ExecuteScalarAsync() != null)
                    return;

                Command.CommandText =
                    "CREATE TABLE Users" +
                    "(ID BIGINT UNSIGNED NOT NULL, " +
                    "Experience MEDIUMINT UNSIGNED NOT NULL, " +
                    "Level SMALLINT UNSIGNED NOT NULL, " +
                    "Global INT UNSIGNED NOT NULL, " +
                    "Description varchar(50) NOT NULL, " +
                    "Insignias varchar(100) NOT NULL, " +
                    "LastChat timestamp NOT NULL DEFAULT NOW() ON UPDATE CURRENT_TIMESTAMP, " +
                    "PRIMARY KEY (ID))";

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }
        }

        private async Task CheckTagsTable()
        {
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SHOW TABLES LIKE 'Tags'";

                await Connection.OpenAsync();

                if (await Command.ExecuteScalarAsync() != null)
                    return;
                
                Command.CommandText =
                    "CREATE TABLE Tags" +
                    "(ID varchar(20) NOT NULL, " +
                    "Body varchar(3000) NOT NULL, " +
                    "PRIMARY KEY (ID))";

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }
        }

        private async Task CheckInsignasTable()
        {
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SHOW TABLES LIKE 'Insignias'";

                await Connection.OpenAsync();

                if (await Command.ExecuteScalarAsync() != null)
                    return;
                
                Command.CommandText =
                    "CREATE TABLE Insignias" +
                    "(ID varchar(20) NOT NULL, " +
                    "Name varchar(20) NOT NULL, " +
                    "URL varchar(30) NOT NULL, " +
                    "PRIMARY KEY (ID))";

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }
        }

        #endregion
        
        #region Connections
        
        public static Connection AggregateConnection(String Input)
        {
            switch (Input.ToUpperInvariant())
            {
                case "Y":
                case "YES":
                    Connection Connection = new Connection("", "", "", "", 0);

                    Console.WriteLine("Enter server address:");
                    Connection.Server = Console.ReadLine();
                        
                    Console.WriteLine("Enter database name:");
                    Connection.Database = Console.ReadLine();
                        
                    Console.WriteLine("Enter user name:");
                    Connection.UID = Console.ReadLine();
                        
                    Console.WriteLine("Enter password:");
                    Connection.Password = Console.ReadLine();
                        
                    Console.WriteLine("Enter port:");
                    Connection.Port = int.Parse(Console.ReadLine());

                    return Connection;
                default:
                    return null;
            }
        }
        
        public void SaveConnection() =>
            File.WriteAllText(ConfigManager.ConfigPath("mysql.json"),
                JsonConvert.SerializeObject(ConnectionString, Formatting.Indented));

        public static DatabaseHandler LoadConnection()
        {
            Connection C = JsonConvert.DeserializeObject<Connection>
                (File.ReadAllText(ConfigManager.ConfigPath("mysql.json")));

            return new DatabaseHandler(C);
        }
        
        #endregion
    }
}