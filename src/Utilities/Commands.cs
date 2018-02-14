using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Reflection;
using Discord.Commands;
using System.Text;
using System.Text.RegularExpressions;

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

        public static async Task<Action<EmbedFieldBuilder>> CreateEmbedFieldAsync(String Name, object Value,
            bool inline)
        {
            return F =>
            {
                F.IsInline = inline;
                F.Name = Name;
                F.Value = Value;
            };
        }

        #region CreateEmbedAsync

        public static async Task<Embed> CreateEmbedAsync(String Title, Color? Color = null, String ThumbnailURL = null,
            String URL = null, IEnumerable<Action<EmbedFieldBuilder>> Fields = null, String Description = null)
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

            foreach (var Field in Fields)
                Embed.AddField(Field);

            return Embed.Build();
        }

        public static async Task<Embed> CreateEmbedAsync(String Title, IEnumerable<Action<EmbedFieldBuilder>> Fields,
            Color Color) =>
            await CreateEmbedAsync(Title, Color, null, null, Fields);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, Color Color) =>
            await CreateEmbedAsync(Title, Color, null, null, null, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description,
            IEnumerable<Action<EmbedFieldBuilder>> Fields, Color Color) =>
            await CreateEmbedAsync(Title, Color, null, null, Fields, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, Color Color, String URL) =>
            await CreateEmbedAsync(Title, Color, null, URL, null, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, String ThumbnailURL,
            IEnumerable<Action<EmbedFieldBuilder>> Fields, Color Color) =>
            await CreateEmbedAsync(Title, Color, ThumbnailURL, null, Fields, Description);

        public static async Task<Embed> CreateEmbedAsync(String Title, String Description, Color Color, String URL,
            String ThumbnailURL) =>
            await CreateEmbedAsync(Title, Color, ThumbnailURL, URL, null, Description);

        #endregion

        public static async Task CreateCommandField(List<Action<EmbedFieldBuilder>> x, MethodInfo m)
        {
            SummaryAttribute sAttribute = (SummaryAttribute) m.GetCustomAttribute(typeof(SummaryAttribute), false);
            String summary = sAttribute == null ? "No summary provided." : sAttribute.Text;

            CommandAttribute cAttribute = (CommandAttribute) m.GetCustomAttribute(typeof(CommandAttribute), false);
            String command = cAttribute == null ? "Unknown command" : cAttribute.Text;

            var parameters = m.GetParameters();
            StringBuilder usage = new StringBuilder($"~{command} ");

            foreach (var p in parameters)
            {
                usage.Append($"<{p.Name}");

                if (p.DefaultValue != DBNull.Value)
                    usage.Append(" (Optional)");

                usage.Append("> ");
            }

            AliasAttribute aAttribute = (AliasAttribute) m.GetCustomAttribute(typeof(AliasAttribute), false);
            StringBuilder aliases = new StringBuilder("None");

            if (aAttribute != null)
            {
                aliases.Clear();

                foreach (string s in aAttribute.Aliases)
                    aliases.Append($"~{s} ");
            }

            x.Add(await CreateEmbedFieldAsync($"~{command}",
                $"\t{summary}\n\tUsage: ``{usage}``\n\tAliases: {aliases}"));
        }

        // Thank you Gigawiz for this clever solution:
        // https://github.com/SergeyMar/ThePunisher/blob/master/DatabaseManager.cs#L62
        public static DateTime ParseDateFromString(String Input)
        {
            // We will assume that Input has been converted to uppercase before being passed to us
            if (Input.Contains("D"))
            {
                String AllInts = Input.Replace("D", "");
                if (int.TryParse(AllInts, out int tm))
                    return DateTime.Now.AddDays(tm);
            }

            if (Input.Contains("H"))
            {
                String AllInts1 = Input.Replace("H", "");
                if (int.TryParse(AllInts1, out int tm1))
                    return DateTime.Now.AddHours(tm1);
            }

            if (!Input.Contains("M")) return DateTime.MaxValue;

            String AllInts2 = Input.Replace("M", "");
            return int.TryParse(AllInts2, out int tm2) ? DateTime.Now.AddMinutes(tm2) : DateTime.MaxValue;
        }
    }
}
