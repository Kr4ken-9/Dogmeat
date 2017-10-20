using System;

namespace Dogmeat.Config
{
    public class API
    {
        public String DiscordToken;
        
        public String SteamAPIKey;
        
        public API(String Discordtoken, String SteamAPIkey)
        {
            DiscordToken = Discordtoken;
            SteamAPIKey = SteamAPIkey;
        }
    }
}