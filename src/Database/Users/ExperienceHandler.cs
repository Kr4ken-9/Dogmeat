using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Discord.WebSocket;
using Discord;

namespace Dogmeat.Database
{
    public class ExperienceHandler
    {   
        public event EventHandler<ExperienceEventArgs> ExperienceUpdate;

        public void OnExperienceUpdate(UUser User, ushort Experience, SocketMessage Context) =>
            ExperienceUpdate(this, new ExperienceEventArgs(User, Experience, Context));
        
        public async Task IncreaseExperience(ulong ID, ushort Experience, SocketMessage context)
        {
            MySqlCommand Command = Vars.DBHandler.Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.Parameters.AddWithValue("Experience", Experience);
            Command.CommandText = "UPDATE Users SET Experience = Experience + @Experience WHERE ID = @ID";
            await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.NONQUERY);

            UUser user = await Vars.DBHandler.UUIHandler.GetUser(ID);
            if (CalculateLevelThreshold(user.Level) < user.Experience)
            {
                Command.Parameters.AddWithValue("ID", user.ID);
                Command.CommandText = "UPDATE Users SET Level = Level + 1 WHERE ID = @ID";
                await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.NONQUERY);

                Embed Embed = await Utilities.Commands.CreateEmbedAsync("Level Up!",
                Description: $"You leveled up to level {user.Level + 1}!");

                await context.Channel.SendMessageAsync("", embed: Embed);
            }
        }

        public static int CalculateLevelThreshold(int level) => 5 * level * level + 50 * level + 100;

        public static Byte CalculateExperience() => (Byte) Vars.Random.Next(0, 11);
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