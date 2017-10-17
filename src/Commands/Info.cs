using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class Info : ModuleBase
    {
        [Command("info"), Summary("Prints info regarding Dogmeat.")]
        public async Task InfoAsync()
        {
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Creation Date", "1/22/17"),
                await Utilities.Commands.CreateEmbedFieldAsync("Version",
                    Assembly.GetEntryAssembly().GetName().Version),
                await Utilities.Commands.CreateEmbedFieldAsync("Guilds", Vars.Client.Guilds.Count),
                await Utilities.Commands.CreateEmbedFieldAsync("Users", await Utils.GetAllUsers()),
                await Utilities.Commands.CreateEmbedFieldAsync("Latest Commit", $"https://github.com/Ex-Presidents/Dogmeat/commit/{Vars.LatestCommit}")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync("Dogmeat Summary", Colors.SexyBlue,
                "https://cdn.discordapp.com/app-icons/272798023816445955/fdef8956d05fdb4d04b0ccbb811c2fc5.jpg",
                "https://ex-presidents.github.io/Dogmeat", Fields.ToArray());
            
            await ReplyAsync("", embed: Embed);
        }
    }
}
