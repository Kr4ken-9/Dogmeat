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
        [Command("rank"), Summary("Displays rank/exp info")]
        public async Task Rank(IUser target = null)
        {
            UUser user = await Vars.DBHandler.UUIHandler.GetUser(target?.Id ?? Context.User.Id);
            long rank = await Vars.DBHandler.UUIHandler.ExpHandler.GetRank(user.ID);

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Level", user.Level),
                await Utilities.Commands.CreateEmbedFieldAsync("Experience",
                    $"{user.Experience} / {ExperienceHandler.CalculateLevelThreshold(user.Level)}"),
                await Utilities.Commands.CreateEmbedFieldAsync("Rank", $"#{rank}")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync(
                (target == null ? Context.User.Username : target.Username) + "'s Ranking",
                Fields.ToArray(), Discord.Color.Default);

            ReplyAsync("", embed: Embed);
        }
    }
}
