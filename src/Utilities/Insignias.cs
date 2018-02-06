using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dogmeat.Database;
using Microsoft.EntityFrameworkCore;

namespace Dogmeat.Utilities
{
    public class Insignias
    {
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
    }
}