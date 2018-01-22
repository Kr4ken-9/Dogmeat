using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Discord.WebSocket;
using Discord;

namespace Dogmeat.Database
{
    public class ExperienceHandler
    {
        public String ConnectionString;

        public ExperienceHandler(String connectionString) => ConnectionString = connectionString;

        public event EventHandler<ExperienceEventArgs> ExperienceUpdate;

        public void OnExperienceUpdate(UUser User, ushort Experience, SocketMessage Context) =>
            ExperienceUpdate(this, new ExperienceEventArgs(User, Experience, Context));

        public async Task IncreaseExperience(ulong ID, ushort Experience, SocketMessage context)
        {
            await AddExperience(ID, Experience);

            UUser user = await Vars.DBHandler.UUIHandler.GetUser(ID);
            if (CalculateLevelThreshold(user.Level) < user.Experience)
            {
                await IncrementLevel(user.ID);

                Embed Embed = await Utilities.Commands.CreateEmbedAsync("Level Up!",
                $"You leveled up to level {user.Level + 1}!", Discord.Color.Default);

                await context.Channel.SendMessageAsync("", embed: Embed);
            }
        }

        public static int CalculateLevelThreshold(int level) => 5 * level * level + 50 * level + 100;

        public static Byte CalculateExperience() => (Byte)Vars.Random.Next(0, 11);

        public async Task AddExperience(ulong ID, ushort Experience)
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.Parameters.AddWithValue("Experience", Experience);
                    Command.CommandText = "UPDATE Users SET Experience = Experience + @Experience WHERE ID = @ID";

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task IncrementLevel(ulong ID)
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.CommandText = "UPDATE Users SET Level = Level + 1 WHERE ID = @ID";

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

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
        public UUser User;
        public ushort Amount;
        public SocketMessage Context;

        public ExperienceEventArgs(UUser user, ushort amount, SocketMessage context)
        {
            User = user;
            Amount = amount;
            Context = context;
        }
    }
}