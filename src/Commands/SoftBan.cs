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
            switch (await Utilities.Commands.CheckMasterAsync(Context.Guild, Context.User))
            {
                case EMaster.NONE:
                    ReplyAsync("I have no master on this server.");
                    return;
                case EMaster.FALSE:
                    ReplyAsync("You must be my master to execute this command.");
                    return;
            }
            
            ReplyAsync($"{User.Mention} is no more.");

            if (Reason == null)
                User.Guild.AddBanAsync(User);
            else
                User.Guild.AddBanAsync(User, 0, Reason);
        }
    }
}
