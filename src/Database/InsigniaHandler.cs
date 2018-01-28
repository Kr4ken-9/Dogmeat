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
            using (DatabaseHandler Context = new DatabaseHandler())
            {
                await Context.Database.EnsureCreatedAsync();

                Insignia insignia = new Insignia(ID, Name, URL);
                await Context.Insignias.AddAsync(insignia);
                await Context.SaveChangesAsync();
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
            using (DatabaseHandler Context = new DatabaseHandler())
            {
                await Context.Database.EnsureCreatedAsync();

                return await Context.Users.AnyAsync(x => x.ID.ToString() == ID);
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