using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Dogmeat.Database
{
    public class InsigniaHandler
    {
        private String connectionString;

        public string ConnectionString { get => connectionString; }

        public InsigniaHandler(String ConnectionString) => connectionString = ConnectionString;

        public async Task AddInsignia(String ID, String Name, String URL = "None")
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.Parameters.AddWithValue("Name", Name);
                    Command.Parameters.AddWithValue("URL", URL);
                    Command.CommandText = "INSERT INTO Insignias VALUES(@ID, @Name, @URL)";

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<IEnumerable<Insignia>> GetInsignias(String IDs)
        {
            List<Insignia> Insignias = new List<Insignia>();
            String[] ids = IDs.Split(';');

            using (DatabaseHandler Context = new DatabaseHandler())
            {
                await Context.Database.EnsureCreatedAsync();
                
                foreach (String ID in ids)
                    Insignias.Add(await Context.Insignias.FirstOrDefaultAsync(i => i.ID == ID));
            }

            return Insignias;
        }

        public async Task<bool> CheckInsignia(String ID)
        {
            bool Exists = false;
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.CommandText = "SELECT EXISTS(SELECT 1 FROM Insignias WHERE ID = @ID LIMIT 1);";

                    object Result = await Command.ExecuteScalarAsync();

                    if (Result == null) return Exists;

                    Int32.TryParse(Result.ToString(), out int exists);

                    Exists = exists != 0;

                    return Exists;
                }
            }
        }
    }

    public class Insignia
    {
        private String url;
        private String id;
        private String name;

        public string ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string URL { get => url; set => url = value; }
        
        // Entity Framework requires parameterless constructor
        public Insignia() {}

        public Insignia(String Id, String name, String url)
        {
            id = Id;
            name = name;
            url = url;
        }
    }
}