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

            //CheckTables();
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
        
        #region Users
        
        public async Task AddUser(UUser User) => AddUser(User.ID, User.Experience, User.Description);

        public async Task AddUser(ulong ID, ushort Experience = 0, String Description = "none")
        {
            if (Description.Length > 30)
                throw new Exception("Description limit is thirty characters.");
            
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.Parameters.AddWithValue("Experience", Experience);
            Command.Parameters.AddWithValue("Description", Description);
            Command.CommandText = "INSERT INTO Users VALUES(@ID, @Experience, 0, 0, @Description)";

            await Utilities.MySql.ExecuteCommand(Connection, Command, Utilities.MySql.CommandExecuteType.NONQUERY);
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
            
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.CommandText = "SELECT EXISTS(SELECT 1 FROM Users WHERE ID = @ID LIMIT 1);";

            object Result =
                await Utilities.MySql.ExecuteCommand(Connection, Command, Utilities.MySql.CommandExecuteType.SCALAR);
            
            if (Result != null)
            {
                Int32.TryParse(Result.ToString(), out int exists);

                Exists = exists != 0;
            }
            
            return Exists;
        }
        
        #endregion

        public async Task AddTag(String ID, String Body)
        {
            if(ID.Length > 10)
                throw new Exception("ID limit is ten characters");
            
            if(Body.Length > 50)
                throw new Exception("Description limit is fifty characters");

            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.Parameters.AddWithValue("Body", Body);
            Command.CommandText = "INSERT INTO Tags VALUES(@ID, @Body)";

            await Utilities.MySql.ExecuteCommand(Connection, Command, Utilities.MySql.CommandExecuteType.NONQUERY);
        }
        
        public async Task<bool> CheckTag(String ID)
        {
            bool Exists = false;
            
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.CommandText = "SELECT EXISTS(SELECT 1 FROM Tags WHERE ID = @ID LIMIT 1);";

            object Result =
                await Utilities.MySql.ExecuteCommand(Connection, Command, Utilities.MySql.CommandExecuteType.SCALAR);
            
            if (Result != null)
            {
                Int32.TryParse(Result.ToString(), out int exists);

                Exists = exists != 0;
            }
            
            return Exists;
        }
        
        public async Task<String> GetTag(String ID)
        {
            String Body = "";
            
            try
            {
                MySqlCommand Command = Connection.CreateCommand();
                Command.Parameters.AddWithValue("ID", ID);
                Command.CommandText = "SELECT * FROM Tags WHERE ID = @ID";

                await Connection.OpenAsync();

                using (MySqlDataReader Reader = Command.ExecuteReader())
                    while (await Reader.ReadAsync())
                        Body = Reader.GetString(1);
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }

            return Body;
        }
        
        #region Tables

        internal async Task CheckTables()
        {
            await CheckUsersTable();
            await CheckTagsTable();
        }
        
        internal async Task CheckUsersTable()
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

        internal async Task CheckTagsTable()
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
                    "(ID varchar(10) NOT NULL, " +
                    "Body varchar(50) NOT NULL, " +
                    "PRIMARY KEY (ID))";

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Connection.Close(); }
        }

        #endregion
        
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
    }
}