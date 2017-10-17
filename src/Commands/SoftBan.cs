using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class SoftBan : ModuleBase
    {
        [Command("softban"), Summary("Softbans a user")]
        public async Task SoftBanAsync([Summary("User to softban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;
            
            ReplyAsync($"{User.Mention} is no more.");

            if (Reason == null)
                User.Guild.AddBanAsync(User);
            else
                User.Guild.AddBanAsync(User, 0, Reason);
        }
    }
}
