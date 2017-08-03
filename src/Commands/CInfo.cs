using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CInfo : ModuleBase
    {
        [Command("info"), Summary("Prints info regarding Dogmeat.")]
        public async Task Info()
        {
            await ReplyAsync("", embed: new EmbedBuilder()
                    {
                        Title = "Dogmeat Summary",
                        Color = Colors.SexyBlue,
                        Url = "https://ex-presidents.github.io/Dogmeat",
                        ThumbnailUrl = "https://cdn.discordapp.com/app-icons/272798023816445955/fdef8956d05fdb4d04b0ccbb811c2fc5.jpg"
                    }
                    
                    #region Fields

                    .AddField(async F =>
                    {
                        F.IsInline = true;
                        F.Name = "Creation Date";
                        F.Value = "1/22/17";
                    })
                    .AddField(async F =>
                    {
                        F.IsInline = true;
                        F.Name = "Version";
                        F.Value = Assembly.GetEntryAssembly().GetName().Version;
                    })
                    .AddField(async F =>
                    {
                        F.IsInline = true;
                        F.Name = "Guilds";
                        F.Value = Vars.Client.Guilds.Count;
                    })
                    .AddField(async F =>
                    {
                        F.IsInline = true;
                        F.Name = "Users";
                        F.Value = await Utils.GetAllUsers();
                    })
                    .AddField(async F =>
                    {
                        F.IsInline = true;
                        F.Name = "Latest Commit";
                        F.Value = $"https://github.com/Ex-Presidents/Dogmeat/commit/{Vars.LatestCommit}";
                    })
                #endregion
            );
        }
    }
}
