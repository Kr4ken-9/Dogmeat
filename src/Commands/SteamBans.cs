using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Commands
{
    public class SteamBans : ModuleBase
    {
        [Command("steambans"), Summary("Probes a steam account for bans")]
        public async Task SteamBansAsync([Summary("Vanity URL or ID of steam profile")] string name = null)
        {
            SteamCommunityProfileModel Profile = await Utilities.Steam.GetProfile(name);

            PlayerSummaryModel Player = await Utilities.Steam.GetPlayerSummary(Profile);

            PlayerBansModel BansModel = await Utilities.Steam.GetPlayerBans(Profile);

            String BanC = BanConclusion((byte)(BansModel.NumberOfGameBans + BansModel.NumberOfVACBans));

            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Ban Conclusion", BanC),
                await Utilities.Commands.CreateEmbedFieldAsync("Community Banned", BansModel.CommunityBanned),
                await Utilities.Commands.CreateEmbedFieldAsync("VAC Banned", BansModel.VACBanned),
                await Utilities.Commands.CreateEmbedFieldAsync("Economy Bans", BansModel.EconomyBan),
                await Utilities.Commands.CreateEmbedFieldAsync("Ban Count",
                    $"Game Bans: {BansModel.NumberOfGameBans}, VAC Bans: {BansModel.NumberOfVACBans}"),
                await Utilities.Commands.CreateEmbedFieldAsync("Last Ban", $"{BansModel.DaysSinceLastBan} days ago.")
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync($"Ban summary for {Player.Nickname}", BanColor(BanC),
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