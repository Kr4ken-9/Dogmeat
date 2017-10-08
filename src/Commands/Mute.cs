using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class Mute : ModuleBase
    {
        [Command("mute"), Summary("Mutes a user")]
        public async Task MuteAsync([Summary("User to mute")] IGuildUser User)
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

            IRole Muted = null;

            if (Utils.GetMutedRole(Context.Guild) == null)
            {
                Muted = await Utilities.Commands.CreateMutedRole(Context.Guild);
                ReplyAsync("Muted role created.");
            }

            if (Muted == null)
                Muted = Utils.GetMutedRole(Context.Guild);
            
            if (!User.RoleIds.Contains(Muted.Id))
            {
                User.AddRoleAsync(Muted);
                ReplyAsync($"{User.Mention} has been muted.");
            }
            else
            {
                User.RemoveRoleAsync(Muted);
                ReplyAsync($"{User.Mention} has been unmuted.");
            }
        }
    }
}