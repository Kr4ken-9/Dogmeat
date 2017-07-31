using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Dogmeat.Commands
{
    public class CHelp : ModuleBase
    {
        [Command("help"), Summary("Dogmeat is a free spirit, and only responds to exact circumstances.")]
        public async Task HelpAsync()
        {
            IDMChannel DMChannel = await Context.User.GetOrCreateDMChannelAsync();

            await DMChannel.SendMessageAsync("", embed: new EmbedBuilder()
                {
                    Title = "Dogmeat Manual",
                    Color = new Color(0, 112, 255),
                    Url = "https://ex-presidents.github.io/Dogmeat",
                    ThumbnailUrl =
                        "https://cdn.discordapp.com/app-icons/272798023816445955/fdef8956d05fdb4d04b0ccbb811c2fc5.jpg"
                }
                
                #region Fields
                
            .AddField(async F =>
                {
                    F.IsInline = true;
                    F.Name = "Commands";
                    F.Value = "~Help, ~Info, ~Meme, ~Steambans [Link], ~Steamprofile [Link]";
                })
             .AddField(async F =>
                {
                    F.IsInline = true;
                    F.Name = "Administrative Commands";
                    F.Value = "~Ban [User], ~Kick [User], ~Mute [User], ~Purge [Amount] [User], ~Softban [User]";
                })
            .AddField(async F =>
                {
                    F.IsInline = true;
                    F.Name = "Using administrative commands";
                    F.Value = "Administrative commands can only be issued by users in a role named \"Master.\"";
                })
            .AddField(async F =>
                {
                    F.IsInline = true;
                    F.Name = "How to disable replies";
                    F.Value = "No. My offensive replies are one of my main features. If you don't like it, kick me.";
                })
            .AddField(async F =>
                {
                    F.IsInline = true;
                    F.Name = "Disclaimer";
                    F.Value = "I am currently in a closed alpha, meaning I am unreliable. Ergo, do not expect me to be reliable.";
                })
            #endregion
            );
        }
    }
}
