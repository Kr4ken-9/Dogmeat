using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;
using System.Linq;

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
                await Utilities.Commands.CreateEmbedFieldAsync("Using Administrative Commands",
                    "Administrative commands can only be issued by users in a role named \"Master.\""),
                await Utilities.Commands.CreateEmbedFieldAsync("How To Disable Replies",
                    "No. My offensive replies are one of my main features. If you don't like it, kick me."),
                await Utilities.Commands.CreateEmbedFieldAsync("Disclaimer",
                    "I am currently in a closed alpha, meaning I am unreliable. Ergo, do not expect me to be reliable."),
                // "\u200B" is the unicode character for a zero width space.
                await Utilities.Commands.CreateEmbedFieldAsync("\u200B", "**Commands**")
            };

            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all methods in this assembly with CommandAttribute.
            // All of our commands have CommandAttribute.

            var commands = assembly.GetTypes()
                .SelectMany(x => x.GetMethods())
                .Where(x => x.GetCustomAttribute(typeof(CommandAttribute), false) != null)
                .ToArray();

            foreach(MethodInfo x in commands)
                await CreateCommandField(Fields, x);

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

        public async Task CreateCommandField(List<Action<EmbedFieldBuilder>> x, MethodInfo m)
        {
            SummaryAttribute sAttribute = (SummaryAttribute)m.GetCustomAttribute(typeof(SummaryAttribute), false);
            String summary = sAttribute == null ? "No summary provided." : sAttribute.Text;

            CommandAttribute cAttribute = (CommandAttribute)m.GetCustomAttribute(typeof(CommandAttribute), false);
            String command = cAttribute == null ? "Unknown command" : cAttribute.Text;

            System.Reflection.ParameterInfo[] parameters = m.GetParameters();
            String usage = $"~{command}";
            foreach (System.Reflection.ParameterInfo p in parameters)
            {
                String arg = " ";
                arg += $"<{p.Name}";
                if (p.DefaultValue != DBNull.Value)
                    arg += " (Optional)";
                arg += ">";
                usage += arg;
            }

            String aliases;
            AliasAttribute aAttribute = (AliasAttribute)m.GetCustomAttribute(typeof(AliasAttribute), false);
            if (aAttribute != null)
            {
                aliases = "";
                foreach (string s in aAttribute.Aliases)
                    aliases += $"~{s} ";
            }
            else
                aliases = "None";
                    
            Action<EmbedFieldBuilder> e = await Utilities.Commands.CreateEmbedFieldAsync($"~{command}",
                $"\t{summary}\n\tUsage: ``{usage}``\n\tAliases: {aliases}");

            x.Add(e);
        }
    }
}