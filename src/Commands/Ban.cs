using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class Ban : ModuleBase
    {
        [Command("ban"), Summary("Bans a user")]
        public async Task BanAsync([Summary("User of person to ban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
        {
            switch (await Utils.CheckMasterAsync(Context.Guild, Context.User))
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
                User.Guild.AddBanAsync(User, 7);
            else
                User.Guild.AddBanAsync(User, 7, Reason);
        }
    }
}
