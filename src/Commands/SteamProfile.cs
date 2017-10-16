using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Utilities;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Commands
{
    public class SteamProfile : ModuleBase
    {
        [Command("steamprofile"), Summary("Gets basic steam profile information based on input")]
        public async Task SteamProfileAsync([Summary("Vanity URL or ID of steam profile")] string name = null)
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
            
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utils.CreateEmbedFieldAsync(true, "SteamID", Player.ProfileUrl),
                await Utils.CreateEmbedFieldAsync(true, "Currently Playing", String.IsNullOrEmpty(Player.PlayingGameName)
                    ? "None" : Player.PlayingGameName),
                await Utils.CreateEmbedFieldAsync(true, "Status", Profile.State)
            };

            Embed Embed = await Utils.CreateEmbedAsync($"Player summary for {Player.Nickname}", Colors.SexyBlue,
                Player.AvatarMediumUrl, Player.ProfileUrl, Fields.ToArray());

            ReplyAsync("", embed: Embed);
        }
    }
}