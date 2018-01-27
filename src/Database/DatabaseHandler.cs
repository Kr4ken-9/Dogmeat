using System;
using System.IO;
using System.Threading.Tasks;
using Dogmeat.Config;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Dogmeat.Database
{
    public class DatabaseHandler : DbContext
    {
        #region Fields/Properties
        
        private String connectionString;

        private UserInfoHandler uuiHandler;

        private TagHandler tags;

        private InsigniaHandler insignias;

        public String ConnectionString { get => connectionString; private set => connectionString = value; }
        public UserInfoHandler UUIHandler { get => uuiHandler; private set => uuiHandler = value; }
        public TagHandler TagHandler { get => tags; private set => tags = value; }
        public InsigniaHandler InsigniaHandler { get => insignias; private set => insignias = value; }
        public DbSet<UUser> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Insignia> Insignias { get; set; }

        #endregion
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConnectionString = $"SERVER={Vars.ConnectionString.Server};" +
                               $"DATABASE={Vars.ConnectionString.Database};" +
                               $"UID={Vars.ConnectionString.UID};" +
                               $"PASSWORD={Vars.ConnectionString.Password};" +
                               $"PORT={Vars.ConnectionString.Port};";

            optionsBuilder.UseMySql(ConnectionString);
            
            UUIHandler = new UserInfoHandler(ConnectionString);
            TagHandler = new TagHandler(ConnectionString);
            InsigniaHandler = new InsigniaHandler(ConnectionString);
            //CheckTables().GetAwaiter().GetResult();
        }

        #region Tables

        private async Task CheckTables()
        {
            await CheckUsersTable();
            await CheckTagsTable();
            await CheckInsignasTable();
        }

        private async Task CheckUsersTable()
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.CommandText = "SHOW TABLES LIKE 'Users'";

                    if (await Command.ExecuteScalarAsync() != null)
                        return;

                    Command.CommandText =
                        "CREATE TABLE Users" +
                        "(ID BIGINT UNSIGNED NOT NULL, " +
                        "Experience MEDIUMINT UNSIGNED NOT NULL, " +
                        "Level SMALLINT UNSIGNED NOT NULL, " +
                        "Description varchar(50) NOT NULL, " +
                        "Insignias varchar(100) NOT NULL, " +
                        "LastChat timestamp NOT NULL DEFAULT NOW() ON UPDATE CURRENT_TIMESTAMP, " +
                        "PRIMARY KEY (ID))";

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task CheckTagsTable()
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.CommandText = "SHOW TABLES LIKE 'Tags'";

                    if (await Command.ExecuteScalarAsync() != null)
                        return;

                    Command.CommandText =
                        "CREATE TABLE Tags" +
                        "(ID varchar(20) NOT NULL, " +
                        "Body varchar(3000) NOT NULL, " +
                        "Owner BIGINT UNSIGNED NOT NULL, " +
                        "PRIMARY KEY (ID))";

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task CheckInsignasTable()
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.CommandText = "SHOW TABLES LIKE 'Insignias'";

                    if (await Command.ExecuteScalarAsync() != null)
                        return;

                    Command.CommandText =
                        "CREATE TABLE Insignias" +
                        "(ID varchar(20) NOT NULL, " +
                        "Name varchar(20) NOT NULL, " +
                        "URL varchar(40) NOT NULL, " +
                        "PRIMARY KEY (ID))";

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

        #endregion

        #region Connections

        public static Connection AggregateConnection(String Input)
        {
            switch (Input.ToUpperInvariant())
            {
                case "Y":
                case "YES":

                    Console.WriteLine("Enter server address:");
                    string server = Console.ReadLine();

                    Console.WriteLine("Enter database name:");
                    string database = Console.ReadLine();

                    Console.WriteLine("Enter user name:");
                    string uid = Console.ReadLine();

                    Console.WriteLine("Enter password:");
                    string password = Console.ReadLine();

                    Console.WriteLine("Enter port:");
                    int port = int.Parse(Console.ReadLine());

                    return new Connection(server, database, uid, password, port);
                default:
                    return null;
            }
        }

        public void SaveConnection() =>
            File.WriteAllText(ConfigManager.ConfigPath("mysql.json"),
                JsonConvert.SerializeObject(Vars.ConnectionString, Formatting.Indented));

        public static void LoadConnection()
        {
            Connection C = JsonConvert.DeserializeObject<Connection>
                (File.ReadAllText(ConfigManager.ConfigPath("mysql.json")));

            Vars.ConnectionString = C;
        }

        #endregion
    }
}