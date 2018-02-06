using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Commands
{
    public class SteamProfile : ModuleBase
    {
        [Command("steamprofile"), Summary("Gets basic steam profile information based on input")]
        public async Task SteamProfileAsync([Summary("Vanity URL or ID of steam profile")] string name = null)
        {
            SteamCommunityProfileModel Profile = await Utilities.Steam.GetProfile(name);

            PlayerSummaryModel Player = await Utilities.Steam.GetPlayerSummary(Profile);
            
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("SteamID", Player.ProfileUrl),
                await Utilities.Commands.CreateEmbedFieldAsync("Currently Playing",
                    String.IsNullOrEmpty(Player.PlayingGameName) ? "None" : Player.PlayingGameName),
                await Utilities.Commands.CreateEmbedFieldAsync("Status", Profile.State)
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync($"Player summary for {Player.Nickname}",
                Colors.SexyBlue, Player.AvatarMediumUrl, Player.ProfileUrl, Fields.ToArray());

            ReplyAsync("", embed: Embed);
        }
    }
}