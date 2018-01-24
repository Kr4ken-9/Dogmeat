using System;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Dogmeat.Config
{
    public static class ConfigManager
    {
        public static String ConfigPath(String File)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CheckDirectory($"{Directory.GetCurrentDirectory()}\\config");
                return $"{Directory.GetCurrentDirectory()}\\config\\{File}";
            }

            CheckDirectory($"{Directory.GetCurrentDirectory()}/config");
            return $"{Directory.GetCurrentDirectory()}/config/{File}";
        }

        private static void CheckDirectory(String Path)
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        public static bool CheckConfigItem(String File) => System.IO.File.Exists(ConfigPath(File));

        public static void SaveConfig()
        {
            API NewAPI = new API(Vars.Token, Vars.SteamAPIKey);
            
            File.WriteAllText($"{ConfigPath("API.json")}", JsonConvert.SerializeObject(NewAPI, Formatting.Indented));
        }

        public static void LoadConfig()
        {
            String Path = ConfigPath("API.json");
            
            if (!File.Exists(Path)) return;

            API SavedAPI = JsonConvert.DeserializeObject<API>(File.ReadAllText(Path));

            Vars.Token = SavedAPI.DiscordToken;
            Vars.SteamAPIKey = SavedAPI.SteamAPIKey;
        }
    }
}