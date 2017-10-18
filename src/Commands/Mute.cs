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
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, User, Context.Channel)) return;

            IRole Muted = null;

            if (Utils.GetMutedRole(Context.Guild) == null)
            {
                Muted = await Utilities.Commands.CreateMutedRoleAsync(Context.Guild);
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