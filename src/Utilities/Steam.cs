using System;
using System.Linq;
using System.Threading.Tasks;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Utilities
{
    public static class Steam
    {
        public static async Task<SteamCommunityProfileModel> GetProfile(String Input)
        {
            ulong ID = (await Vars.SteamInterface.ResolveVanityUrlAsync(Input)).Data;

            if (ID == null) return null;

            return await Vars.SteamInterface.GetCommunityProfileAsync(ID);
        }

        public static async Task<PlayerSummaryModel> GetPlayerSummary(SteamCommunityProfileModel Profile) =>
            (await Vars.SteamInterface.GetPlayerSummaryAsync(Profile.SteamID)).Data;
        
        #region Bans
        
        public static async Task<PlayerBansModel> GetPlayerBans(ulong ID) =>
            (await Vars.SteamInterface.GetPlayerBansAsync(ID)).Data.FirstOrDefault();

        public static async Task<PlayerBansModel> GetPlayerBans(SteamCommunityProfileModel Profile) =>
            await GetPlayerBans(Profile.SteamID);
        
        #endregion
    }
}