using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Dogmeat.Utilities
{
    public class Commands
    {
        public static async Task<EMaster> CheckMasterAsync(IGuild Guild, IUser User)
        {
            if (Utils.GetMasterRole(Guild) == null)
                return EMaster.NONE;

            return !((SocketGuildUser) User).Roles.Contains(Utils.GetMasterRole(Guild))
                ? EMaster.FALSE
                : EMaster.TRUE;
        }
        
        public static async Task<IRole> CreateMutedRole(IGuild Guild)
        {
            IRole Muted = await Guild.CreateRoleAsync("Muted", Vars.MutedPermissions, Color.Red);

            foreach (SocketTextChannel Channel in ((SocketGuild) Guild).TextChannels)
                Channel.AddPermissionOverwriteAsync(Muted, Vars.MutedChannelPermissions);

            return Muted;
        }
    }
}