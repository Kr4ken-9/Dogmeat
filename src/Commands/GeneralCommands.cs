using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class GeneralCommands : ModuleBase
    {
        [Command("help"), Summary("Dogmeat is a free spirit, and only responds to exact circumstances.")]
        public async Task Help()
        {
            IDMChannel DMChannel = await Context.User.GetOrCreateDMChannelAsync();

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Commands",
                    "~Help, ~Info, ~Meme, ~Steambans [Link], ~Steamprofile [Link]"),
                await Utilities.Commands.CreateEmbedFieldAsync("Administrative Commands",
                    "~Ban [User], ~Kick [User], ~Mute [User], ~Purge [Amount] [User], ~Softban [User]"),
                await Utilities.Commands.CreateEmbedFieldAsync("Using Administrative Commands",
                    "Administrative commands can only be issued by users in a role named \"Master.\""),
                await Utilities.Commands.CreateEmbedFieldAsync("How To Disable Replies",
                    "No. My offensive replies are one of my main features. If you don't like it, kick me."),
                await Utilities.Commands.CreateEmbedFieldAsync("Disclaimer",
                    "I am currently in a closed alpha, meaning I am unreliable. Ergo, do not expect me to be reliable.")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync("Dogmeat Manual", Colors.SexyBlue,
                "https://cdn.discordapp.com/app-icons/272798023816445955/fdef8956d05fdb4d04b0ccbb811c2fc5.jpg",
                "https://ex-presidents.github.io/Dogmeat", Fields.ToArray());

            await DMChannel.SendMessageAsync("", embed: Embed);
        }
        
        [Command("info"), Summary("Prints info regarding Dogmeat.")]
        public async Task Info()
        {
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Creation Date", "1/22/17"),
                await Utilities.Commands.CreateEmbedFieldAsync("Version",
                    Assembly.GetEntryAssembly().GetName().Version),
                await Utilities.Commands.CreateEmbedFieldAsync("Guilds", Vars.Client.Guilds.Count),
                await Utilities.Commands.CreateEmbedFieldAsync("Users", await Misc.GetAllUsers()),
                await Utilities.Commands.CreateEmbedFieldAsync("Latest Commit",
                    $"https://github.com/Ex-Presidents/Dogmeat/commit/{Vars.LatestCommit}")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync("Dogmeat Summary", Colors.SexyBlue,
                "https://cdn.discordapp.com/app-icons/272798023816445955/fdef8956d05fdb4d04b0ccbb811c2fc5.jpg",
                "https://ex-presidents.github.io/Dogmeat", Fields.ToArray());
            
            await ReplyAsync("", embed: Embed);
        }
        
        [Command("meme"), Summary("Only the dankest")]
        public async Task Meme() => ReplyAsync(Vars.Memes[Vars.Random.Next(0, Vars.Memes.Length + 1)]);
    }
}