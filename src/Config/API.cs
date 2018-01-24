using System;

namespace Dogmeat.Config
{
    public class API
    {
        private String discordToken;
        private String steamAPIKey;

        public string DiscordToken { get => discordToken;}
        public string SteamAPIKey { get => steamAPIKey; }

        public API(String Discordtoken, String SteamAPIkey)
        {
            discordToken = Discordtoken;
            steamAPIKey = SteamAPIkey;
        }
    }
}