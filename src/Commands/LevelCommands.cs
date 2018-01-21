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
            
            MySqlCommand Command = Vars.DBHandler.Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", user.ID);
            Command.CommandText =
                "SELECT COUNT(*) AS rank FROM Users WHERE Experience > (SELECT Experience FROM Users WHERE ID = @ID)";

            object result = await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.SCALAR);
            long rank;
            if (result == null)
            {
                ReplyAsync("The query failed for some reason...");
                return;
            }

            rank = (long)result;

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Level", user.Level),
                await Utilities.Commands.CreateEmbedFieldAsync("Experience",
                    $"{user.Experience} / {ExperienceHandler.CalculateLevelThreshold(user.Level)}"),
                await Utilities.Commands.CreateEmbedFieldAsync("Rank", $"#{rank + 1}")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync(
                (target == null ? Context.User.Username : target.Username) + "'s Ranking",
                Color.Default, Fields: Fields.ToArray());

            ReplyAsync("", embed: Embed);
        }
    }
}
