using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class Kick : ModuleBase
    {
        [Command("kick"), Summary("Kicks specified user")]
        public async Task KickAsync([Summary("User to kick")] IGuildUser User, [Summary("Reason for kick")] String Reason = null)
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
            User.KickAsync(Reason);
        }
    }
}
