using System.IO;
using Newtonsoft.Json;

namespace Dogmeat.Config
{
    public class ConfigManager
    {
        public static void SaveConfig()
        {
            API NewAPI = new API(Vars.Token, Vars.SteamAPIKey);
            
            File.WriteAllText("config//API.json", JsonConvert.SerializeObject(NewAPI, Formatting.Indented));
        }

        public static void LoadConfig()
        {
            if (!File.Exists("config//API.json")) return;

            API SavedAPI = JsonConvert.DeserializeObject<API>(File.ReadAllText("config//API.json"));

            Vars.Token = SavedAPI.DiscordToken;
            Vars.SteamAPIKey = SavedAPI.SteamAPIKey;
        }
    }
}