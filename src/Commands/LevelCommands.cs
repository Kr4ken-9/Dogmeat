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

            UUser User;

            using (DatabaseHandler Context = new DatabaseHandler())
            {
                User = Context.Users.First(user => user.ID == targetUser.Id);

                if (User == null)
                {
                    ReplyAsync($"{targetUser.Mention} is not in the database.");
                    return;
                }
            }

            int rank = await Vars.DBHandler.uuiHandler.ExpHandler.GetRank(User.ID);

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Level", User.Level),
                await Utilities.Commands.CreateEmbedFieldAsync("Experience",
                    $"{User.Experience} / {ExperienceHandler.CalculateLevelThreshold(User.Level)}"),
                await Utilities.Commands.CreateEmbedFieldAsync("Rank", $"#{rank}")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync(
                target.Username + "'s Profile", User.Description,
                targetUser.GetAvatarUrl(), Fields.ToArray(), Discord.Color.Default);

            ReplyAsync("", embed: Embed);
        }
    }
}
