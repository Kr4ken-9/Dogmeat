using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CKick : ModuleBase
    {
        [Command("kick"), Summary("Kicks specified user")]
        public async Task KickAsync([Summary("User to kick")] IGuildUser User, [Summary("Reason for kick")] String Reason = null)
        {
            if (Utils.GetMasterRole((SocketGuild) Context.Guild) == null)
            {
                ReplyAsync("I have no master on this server.");
                return;
            }

            if (!((SocketGuildUser) Context.User).Roles.Contains(Utils.GetMasterRole((SocketGuild) Context.Guild)))
            {
                await ReplyAsync("You must be my master to execute this command.");
                return;
            }

            User.KickAsync(Reason);
            ReplyAsync($"{User.Username} is no more.");
        }
    }
}
