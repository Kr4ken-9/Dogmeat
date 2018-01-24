using Discord;
using Discord.Commands;
using Dogmeat.Database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

            if (!await Vars.DBHandler.UUIHandler.CheckUser(targetUser.Id))
            {
                ReplyAsync($"{targetUser.Mention} is not in the database.");
                return;
            }

            UUser user = await Vars.DBHandler.UUIHandler.GetUser(targetUser.Id);
            long rank = await Vars.DBHandler.UUIHandler.ExpHandler.GetRank(user.ID);

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

            ReplyAsync("", embed: Embed);
        }
    }
}
