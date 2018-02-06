using System;
using System.IO;
using Dogmeat.Config;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dogmeat.Database
{
    public class DatabaseHandler : DbContext
    {
        #region Fields/Properties
        
        public DbSet<UUser> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Insignia> Insignias { get; set; }

        #endregion
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql($"SERVER={Vars.ConnectionString.Server};" +
                                    $"DATABASE={Vars.ConnectionString.Database};" +
                                    $"UID={Vars.ConnectionString.UID};" +
                                    $"PASSWORD={Vars.ConnectionString.Password};" +
                                    $"PORT={Vars.ConnectionString.Port};");
        }

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