using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CSoftBan : ModuleBase
    {
        [Command("softban"), Summary("Softbans a user")]
        public async Task SoftBanAsync([Summary("User to softban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
        {
            if (Utils.GetMasterRole((SocketGuild) Context.Guild) == null)
            {
                ReplyAsync("I have no master on this server.");
                return;
            }
            else if (!((SocketGuildUser) Context.User).Roles.Contains(Utils.GetMasterRole((SocketGuild) Context.Guild)))
            {
                ReplyAsync("You must be my master to execute this command.");
                return;
            }

            if (Reason == null)
                User.Guild.AddBanAsync(User);
            else
                User.Guild.AddBanAsync(User, 0, Reason);
            
            ReplyAsync($"{User.Nickname ?? User.Username} is no more.");
        }
    }
}
