using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class Ban : ModuleBase
    {
        [Command("ban"), Summary("Bans a user")]
        public async Task BanAsync([Summary("User of person to ban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;
            
            ReplyAsync($"{User.Mention} is no more.");

            if (Reason == null)
                User.Guild.AddBanAsync(User, 7);
            else
                User.Guild.AddBanAsync(User, 7, Reason);
        }
    }
}
