using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Discord.WebSocket;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Dogmeat.Database
{
    public class ExperienceHandler
    {
        private String connectionString;

        public string ConnectionString { get => connectionString; }

        public ExperienceHandler(String ConnectionString) => connectionString = ConnectionString;

        public event EventHandler<ExperienceEventArgs> ExperienceUpdate;

        public void OnExperienceUpdate(UUser User, ushort Experience, SocketMessage Context) =>
            ExperienceUpdate(this, new ExperienceEventArgs(User, Experience, Context));

        public async Task IncreaseExperience(ulong ID, ushort Experience, SocketMessage MessageContext)
        {
            using (DatabaseHandler Context = new DatabaseHandler())
            {
                UUser User = await Context.Users.FirstAsync(user => user.ID == ID);
                await Context.AddAsync(User);
                
                User.Experience += Experience;

                if (CalculateLevelThreshold(User.Level) < User.Experience)
                {
                    User.Level++;

                    Embed Embed = await Utilities.Commands.CreateEmbedAsync("Level Up!",
                        $"You leveled up to level {User.Level}!", Colors.SexyOrange);

                    MessageContext.Channel.SendMessageAsync("", embed: Embed);
                }
                
                await Context.SaveChangesAsync();
            }
        }

        public static ushort CalculateLevelThreshold(int level) => (ushort)(5 * level * level + 50 * level + 100);

        public static Byte CalculateExperience() => (Byte)Vars.Random.Next(0, 11);

        public async Task<long> GetRank(ulong ID)
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.CommandText =
                        "SELECT COUNT(*) AS rank FROM Users WHERE Experience > (SELECT Experience FROM Users WHERE ID = @ID)";
                    object res = await Command.ExecuteScalarAsync();

                    return (long)res + 1;
                }
            }
        }
    }

    public class ExperienceEventArgs : EventArgs
    {
        private UUser user;
        private ushort amount;
        private SocketMessage context;

        public UUser User { get => user; }
        public ushort Amount { get => amount; }
        public SocketMessage Context { get => context; }

        public ExperienceEventArgs(UUser User, ushort Amount, SocketMessage Context)
        {
            user = User;
            amount = Amount;
            context = Context;
        }
    }
}