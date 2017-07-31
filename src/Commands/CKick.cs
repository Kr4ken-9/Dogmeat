using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CKick : ModuleBase
    {
        [Command("kick"), Summary("Kicks a player by id or name")]
        public async Task KickAsync([Summary("id or name of player to kick")] string Input)
        {
            if(Utils.GetMasterRole((SocketGuild)Context.Guild) == null)
                await ReplyAsync("I have no master on this server.");
            else if (!((SocketGuildUser)Context.User).Roles.Contains(Utils.GetMasterRole((SocketGuild)Context.Guild)))
                await ReplyAsync("You must be my master to execute this command.");
            else if (Context.Message.MentionedUserIds.Count != 0 && Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault()) != null)
            {
                await (await Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault())).KickAsync();
                await ReplyAsync(Input + " is no more.");
            }
            else if (await Utils.GetUser(Context.Guild, Input) != null)
            {
                await (await Utils.GetUser(Context.Guild, Input)).KickAsync();
                await ReplyAsync(Input + " is no more.");
            }
            else
                await ReplyAsync("Who the fuck is " + Input + "?");
        }
    }
}
