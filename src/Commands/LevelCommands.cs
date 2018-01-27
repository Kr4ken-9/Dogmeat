using Discord;
using Discord.Commands;
using Dogmeat.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dogmeat.Commands
{
    public class LevelCommands : ModuleBase
    {
        [Command("rank"), Summary("Displays rank/exp info"), Alias("profile")]
        public async Task Rank(IUser target = null)
        {
            IUser targetUser = target ?? Context.User;

            using (DatabaseHandler Context = new DatabaseHandler())
            {
                UUser User = Context.Users.First(user => user.ID == targetUser.Id);

                if (User == null)
                {
                    ReplyAsync($"{targetUser.Mention} is not in the database.");
                    return;
                }
            }
            
            //TODO: Bother Extra To Do This
            /*long rank = await Vars.DBHandler.UUIHandler.ExpHandler.GetRank(user.ID);

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Level", user.Level),
                await Utilities.Commands.CreateEmbedFieldAsync("Experience",
                    $"{user.Experience} / {ExperienceHandler.CalculateLevelThreshold(user.Level)}"),
                await Utilities.Commands.CreateEmbedFieldAsync("Rank", $"#{rank}")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync(
                target.Username + "'s Profile", user.Description,
                targetUser.GetAvatarUrl(), Fields.ToArray(), Discord.Color.Default);

            ReplyAsync("", embed: Embed);*/
        }
    }
}
