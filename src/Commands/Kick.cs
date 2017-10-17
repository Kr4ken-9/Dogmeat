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
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;
                
            ReplyAsync($"{User.Mention} is no more.");
            User.KickAsync(Reason);
        }
    }
}
