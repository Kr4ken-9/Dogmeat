using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Dogmeat.Utilities
{
    public static class Commands
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
        
        #region CreateEmbedAsync

        public static async Task<Embed> CreateEmbedAsync(String Title, Color? Color = null, String ThumbnailURL = null, String URL = null, IEnumerable<Action<EmbedFieldBuilder>> Fields = null, String Description = null)
        {
            EmbedBuilder Embed = new EmbedBuilder
            {
                Title = Title,
                Color = Color ?? Discord.Color.Default,
                ThumbnailUrl = ThumbnailURL,
                Url = URL,
                Description = Description
            };

            if (Fields == null) return Embed.Build();
            
            foreach(var Field in Fields)
                Embed.AddField(Field);

            return Embed.Build();
        }
        
        public static async Task<Embed> CreateEmbedAsync(String Title, IEnumerable<Action<EmbedFieldBuilder>> Fields, Color Color) =>
            await CreateEmbedAsync(Title, Color, null, null, Fields);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, Color Color) =>
            await CreateEmbedAsync(Title, Color, null, null, null, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, IEnumerable<Action<EmbedFieldBuilder>> Fields, Color Color) =>
            await CreateEmbedAsync(Title, Color, null, null, Fields, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, Color Color, String URL) =>
            await CreateEmbedAsync(Title, Color, null, URL, null, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, String ThumbnailURL, IEnumerable<Action<EmbedFieldBuilder>> Fields, Color Color) =>
            await CreateEmbedAsync(Title, Color, ThumbnailURL, null, Fields, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, Color Color, String URL, String ThumbnailURL) =>
            await CreateEmbedAsync(Title, Color, ThumbnailURL, URL, null, Description);
        
        #endregion
    }
}