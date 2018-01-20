using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Utilities
{
    public class Commands
    {
        public static async Task<EMaster> CheckMasterAsync(IGuild Guild, IUser User)
        {
            if (Misc.GetMasterRole(Guild) == null)
                return EMaster.NONE;

            return !((SocketGuildUser) User).Roles.Contains(Misc.GetMasterRole(Guild))
                ? EMaster.FALSE
                : EMaster.TRUE;
        }
        
        public static async Task<IRole> CreateMutedRoleAsync(IGuild Guild)
        {
            IRole Muted = await Guild.CreateRoleAsync("Muted", Vars.MutedPermissions, Color.Red);

            foreach (SocketTextChannel Channel in ((SocketGuild) Guild).TextChannels)
                Channel.AddPermissionOverwriteAsync(Muted, Vars.MutedChannelPermissions);

            return Muted;
        }

        public static async Task<bool> CommandMasterAsync(IGuild Guild, IUser User, IMessageChannel Channel)
        {
            switch (await CheckMasterAsync(Guild, User))
            {
                case EMaster.NONE:
                    Channel.SendMessageAsync("I have no master on this server.");
                    return false;
                case EMaster.FALSE:
                    Channel.SendMessageAsync("You must be my master to execute this command.");
                    return false;
                default:
                    return true;
            }
        }
        
        public static async Task<Action<EmbedFieldBuilder>> CreateEmbedFieldAsync(String Name, object Value)
        {
            return F =>
            {
                F.IsInline = true;
                F.Name = Name;
                F.Value = Value;
            };
        }

        public static async Task<Embed> CreateEmbedAsync(String Title, Color? Color, String ThumbnailURL, String URL, Action<EmbedFieldBuilder>[] Fields = null, string Description = null)
        {
            EmbedBuilder Embed = new EmbedBuilder
            {
                Title = Title,
                Color = Color,
                ThumbnailUrl = ThumbnailURL,
                Url = URL,
                Description = Description
            };

            if (Fields == null) return Embed.Build();
            
            for (int i = 0; i < Fields.Length; i++)
                Embed.AddField(Fields[i]);

            return Embed.Build();
        }
    }
}