using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Commands
{
    public class SteamBans : ModuleBase
    {
        [Command("steambans"), Summary("Probes a steam account for bans")]
        public async Task SteamBansAsync([Summary("Vanity URL or ID of steam profile")] string name = null)
        {
            SteamCommunityProfileModel Profile = await Utilities.Steam.GetProfile(name);
            
            if (Profile == null)
            {
                ReplyAsync("Input was not a valid URL or ID.");
                return;
            }

            if (Profile.VisibilityState != 3)
            {
                ReplyAsync("Player's profile was not public.");
                return;
            }

            PlayerSummaryModel Player = await Utilities.Steam.GetPlayerSummary(Profile);

            PlayerBansModel BansModel = await Utilities.Steam.GetPlayerBans(Profile);

            String BanC = BanConclusion((byte)(BansModel.NumberOfGameBans + BansModel.NumberOfVACBans));

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utils.CreateEmbedFieldAsync(true, "Ban Conclusion", BanC),
                await Utils.CreateEmbedFieldAsync(true, "Community Banned", BansModel.CommunityBanned),
                await Utils.CreateEmbedFieldAsync(true, "VAC Banned", BansModel.VACBanned),
                await Utils.CreateEmbedFieldAsync(true, "Economy Bans", BansModel.EconomyBan),
                await Utils.CreateEmbedFieldAsync(true, "Ban Count",
                    $"Game Bans: {BansModel.NumberOfGameBans}, VAC Bans: {BansModel.NumberOfVACBans}"),
                await Utils.CreateEmbedFieldAsync(true, "Last Ban", $"{BansModel.DaysSinceLastBan} days ago.")
            };

            Embed Embed = await Utils.CreateEmbedAsync($"Ban summary for {Player.Nickname}", BanColor(BanC),
                Player.AvatarMediumUrl, Player.ProfileUrl, Fields.ToArray());
            
            ReplyAsync("", embed: Embed);
        }

        private static String BanConclusion(byte BanCount)
        {
            switch (BanCount)
            {
                case 0:
                    return "Anti-Cheater";
                case 1:
                case 2:
                    return "Ashamed Cheater";
                default:
                    return BanCount <= 3 ? "Proud Cheater" : "Anti-Gamer";
            }
        }

        private static Color BanColor(String BanConclusion)
        {
            switch (BanConclusion)
            {
                case "Anti-Cheater":
                    return Colors.SexyLime;
                case "Ashamed Cheater":
                    return Colors.SexyOrange;
                default:
                    return Color.Red;
            }
        }
    }
}