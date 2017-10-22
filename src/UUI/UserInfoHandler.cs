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
            
            //CheckSchema();
        }

        internal async Task CheckSchema()
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
                    "Description varchar(30) NOT NULL, " +
                    "PRIMARY KEY (ID))";

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }
        }

        public async Task AddUser(UUser User) => AddUser(User.ID, User.Experience, User.Description);

        public async Task AddUser(ulong ID, ushort Experience = 0, String Description = "none")
        {
            if (Description.Length > 30)
                throw new Exception("Description limit is thirty characters.");
            
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.Parameters.AddWithValue("ID", ID);
                Command.Parameters.AddWithValue("Experience", Experience);
                Command.Parameters.AddWithValue("Description", Description);
                Command.CommandText = "INSERT INTO Users VALUES(@ID, @Experience, 0, 0, @Description)";

                await Connection.OpenAsync();

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }
        }

        public async Task<UUser> GetUser(ulong ID)
        {
            UUser User = new UUser(ID, 0, 0, 0, "");
            
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.Parameters.AddWithValue("ID", ID);
                Command.CommandText = "SELECT * FROM Users WHERE ID = @ID";

                await Connection.OpenAsync();

                using (MySqlDataReader Reader = Command.ExecuteReader())
                {
                    while (await Reader.ReadAsync())
                    {
                        User.Experience = (ushort) Reader.GetInt16(1);
                        User.Level = (ushort) Reader.GetInt16(2);
                        User.Global = (uint) Reader.GetInt32(3);
                        User.Description = Reader.GetString(4);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }

            return User;
        }

        public async Task<bool> CheckUser(ulong ID)
        {
            bool Exists = false;
            
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.Parameters.AddWithValue("ID", ID);
                Command.CommandText = "SELECT EXISTS(SELECT 1 FROM Users WHERE ID = @ID LIMIT 1);";

                await Connection.OpenAsync();

                object result = await Command.ExecuteScalarAsync();
                if (result != null)
                {
                    Int32.TryParse(result.ToString(), out int exists);

                    Exists = exists != 0;
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }

            return Exists;
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
                case "Y":
                case "YES":
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
                default:
                    return null;
            }
        }
    }
}