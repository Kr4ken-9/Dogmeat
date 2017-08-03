using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CMute : ModuleBase
    {
        [Command("mute"), Summary("Mutes a user")]
        public async Task Mute([Summary("User to mute")] IGuildUser User)
        {
            if (Utils.GetMasterRole((SocketGuild) Context.Guild) == null)
            {
                ReplyAsync("I have no master on this server.");
                return;
            }
            
            if (!((SocketGuildUser) Context.User).Roles.Contains(Utils.GetMasterRole((SocketGuild) Context.Guild)))
            {
                ReplyAsync("You must be my master to execute this command.");
                return;
            }
            
            if (Utils.GetMutedRole((SocketGuild) Context.Guild) == null)
            {
                Utils.CreateMutedRole((SocketGuild) Context.Guild);
                ReplyAsync("Muted role created.");
            }

            IRole muted = Utils.GetMutedRole((SocketGuild) Context.Guild);

            if (User.RoleIds.Contains(muted.Id))
            {
                User.RemoveRoleAsync(muted);
                ReplyAsync($"{Context.User.Username} unmuted {User.Username}");
            }
            
            else
            {
                User.AddRoleAsync(muted);
                ReplyAsync($"{Context.User.Username} muted {User.Username}");
            }
        }
    }
}