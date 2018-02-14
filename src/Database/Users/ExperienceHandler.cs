using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Dogmeat.Database
{
    public static class ExperienceHandler
    {
        public static async Task IncreaseExperience(UUser User, ushort Experience, SocketMessage MessageContext)
        {
            using (DatabaseHandler Context = new DatabaseHandler())
            {
                await Context.Database.EnsureCreatedAsync();
                Context.Users.Update(User);
                
                User.Experience += Experience;
                
                if (CalculateLevelThreshold(User.Level) < User.Experience)
                {
                    User.Level++;

                    Embed Embed = await Utilities.Commands.CreateEmbedAsync("Level Up!",
                        $"{MessageContext.Author.Mention}, you leveled up to level {User.Level}!", Colors.SexyOrange);

                    MessageContext.Channel.SendMessageAsync("", embed: Embed);
                }
                await Context.SaveChangesAsync();
            }
        }

        public static ushort CalculateLevelThreshold(int level) => (ushort)(5 * level * level + 50 * level + 100);

        public static Byte CalculateExperience() => (Byte)Vars.Random.Next(0, 11);
    }
}