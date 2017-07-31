using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Steam.Models.SteamCommunity;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class CSteamBans : ModuleBase
    {
        [Command("steambans"), Summary("Probes a steam account for bans")]
        public async Task SteamProfile([Summary("Vanity URL or ID of steam profile")] string name = null)
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

            uint BanCount = BansModel.NumberOfGameBans + BansModel.NumberOfVACBans;
            
            ReplyAsync("", embed: new EmbedBuilder()
                {
                    Title = $"Ban summary for {Player.Nickname}",
                    Color = new Color(255, 0, 0),
                    ThumbnailUrl = Player.AvatarMediumUrl,
                    Url = Player.ProfileUrl
                }
                
                #region Fields
                
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Ban Conclusion";
                    if (BanCount == 0)
                        F.Value = "Anti-Cheater";
                    else if (BanCount == 1)
                        F.Value = "Ashamed Cheater";
                    else if (BanCount <= 3)
                        F.Value = "Proud Cheater";
                    else
                        F.Value = "Anti-Gamer";
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Community Banned";
                    F.Value = BansModel.CommunityBanned;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "VAC Banned";
                    F.Value = BansModel.VACBanned;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Economy Bans";
                    F.Value = BansModel.EconomyBan;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Ban Count";
                    F.Value = "Game Bans: " + BansModel.NumberOfGameBans + ", VAC Bans: " + BansModel.NumberOfVACBans;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Last Ban";
                    F.Value = BansModel.DaysSinceLastBan + " days ago.";
                })
            
            #endregion
            );
        }
    }
}