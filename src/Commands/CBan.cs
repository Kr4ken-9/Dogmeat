using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CBan : ModuleBase
    {
        [Command("ban"), Summary("Bans a user")]
        public async Task BanAsync([Summary("User of person to ban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
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
                User.Guild.AddBanAsync(User, 7);
            else
                User.Guild.AddBanAsync(User, 7, Reason);
            
            ReplyAsync($"{User.Nickname ?? User.Username} is no more.");
        }
    }
}
