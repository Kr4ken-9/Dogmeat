using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Commands
{
    public class CSteamProfile : ModuleBase
    {
        [Command("steamprofile"), Summary("Gets basic steam profile information based on input")]
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

            ReplyAsync("", embed: new EmbedBuilder()
                {
                    Title = $"Player summary for {Player.Nickname}",
                    Color = new Color(0, 112, 255),
                    ThumbnailUrl = Player.AvatarMediumUrl,
                    Url = Player.ProfileUrl
                }
                
                #region Fields
                
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "SteamID";
                    F.Value = Player.SteamId;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Most Played Game";
                    F.Value = Profile.MostPlayedGames.First().Name;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Currently Playing";
                    F.Value = String.IsNullOrEmpty(Player.PlayingGameName) ? "None" : Player.PlayingGameName;
                })
                .AddField(F =>
                {
                    F.IsInline = true;
                    F.Name = "Status";
                    F.Value = Profile.State;
                })
            
            #endregion
            );
        }
    }
}