using System;
using System.Linq;
using System.Threading.Tasks;
using Steam.Models.SteamCommunity;

namespace Dogmeat.Utilities
{
    public class Steam
    {
        #region Profiles
        
        public static async Task<SteamCommunityProfileModel> GetProfile(ulong ID) => await Vars.SteamInterface.GetCommunityProfileAsync(ID);

        public static async Task<SteamCommunityProfileModel> GetProfile(PlayerSummaryModel Player) =>
            await Vars.SteamInterface.GetCommunityProfileAsync(Player.SteamId);
        
        public static async Task<SteamCommunityProfileModel> GetProfile(String Input)
        {
            if(ulong.TryParse(Input, out ulong Id))
            {
                SteamCommunityProfileModel Profile = await GetProfile(Id);

                if (Profile != null) return Profile;
            }
            
            ulong ID = (await Vars.SteamInterface.ResolveVanityUrlAsync(Input)).Data;

            if (ID == null) return null;

            return await Vars.SteamInterface.GetCommunityProfileAsync(ID);
        }
        
        #endregion
        
        #region Players

        public static async Task<PlayerSummaryModel> GetPlayerSummary(SteamCommunityProfileModel Profile) =>
            (await Vars.SteamInterface.GetPlayerSummaryAsync(Profile.SteamID)).Data;

        public static async Task<PlayerSummaryModel> GetPlayerSummary(ulong ID) =>
            (await Vars.SteamInterface.GetPlayerSummaryAsync(ID)).Data;

        public static async Task<PlayerSummaryModel> GetPlayerSummary(String CustomURL)
        {
            SteamCommunityProfileModel Profile = await GetProfile(CustomURL);

            if (Profile == null) return null;
            
            return await GetPlayerSummary(Profile);
        }
        
        #endregion
        
        #region Bans
        
        public static async Task<PlayerBansModel> GetPlayerBans(ulong ID) =>
            (await Vars.SteamInterface.GetPlayerBansAsync(ID)).Data.FirstOrDefault();

        public static async Task<PlayerBansModel> GetPlayerBans(SteamCommunityProfileModel Profile) =>
            await GetPlayerBans(Profile.SteamID);

        public static async Task<PlayerBansModel> GetPlayerBans(PlayerSummaryModel Player) =>
            await GetPlayerBans(Player.SteamId);

        public static async Task<PlayerBansModel> GetPlayerBans(String CustomURL)
        {
            SteamCommunityProfileModel Profile = await GetProfile(CustomURL);

            if (Profile == null) return null;

            return await GetPlayerBans(Profile);
        }
        
        #endregion
    }
}