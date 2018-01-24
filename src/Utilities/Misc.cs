using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Dogmeat.Config;

namespace Dogmeat.Utilities
{
    public class Misc
    {
        #region Variables

        private static bool continueShutdown;

        public static bool ContinueShutdown { get => continueShutdown; set => continueShutdown = value; }

        public static IRole GetMasterRole(IGuild Guild) => Guild.Roles.FirstOrDefault(role => role.Name == "Master");

        public static IRole GetMutedRole(IGuild Guild) => Guild.Roles.FirstOrDefault(role => role.Name == "Muted");

        #endregion Variables

        public static async Task<int> GetAllUsers()
        {
            int Users = 0;

            foreach (SocketGuild Guild in Vars.Client.Guilds)
                Users += Guild.Users.Count;

            return Users;
        }
        
        #region Connection

        public static async Task ShutdownAsync()
        {
            Vars.KeepAlive = false;
            ContinueShutdown = true;
            
            ConfigManager.SaveConfig();
            
            Logger.Log("Client is shutting down in five seconds.", ConsoleColor.Red);

            await ShutdownMeatAsync();
        }

        public static async Task ShutdownMeatAsync()
        {
            while (!ContinueShutdown)
            {
                Logger.Log("5", ConsoleColor.Red);
                Thread.Sleep(1000);

                Logger.Log("4", ConsoleColor.Red);
                Thread.Sleep(1000);

                Logger.Log("3", ConsoleColor.Red);
                Thread.Sleep(1000);

                Logger.Log("2", ConsoleColor.Red);
                Thread.Sleep(1000);

                Logger.Log("1", ConsoleColor.Red);
                Thread.Sleep(1000);

                await DisconnectAsync();
                Environment.Exit(0);
            }
        }

        public static async Task DisconnectAsync()
        {
            Vars.KeepAlive = false;
            await Vars.Client.StopAsync();
            Logger.Log("Dogmeat disconnected.", ConsoleColor.Red);
        }
        
        #endregion

        public static async Task UpdateVarsAsync()
        {
            while (Vars.KeepAlive)
            {
                Vars.Answers = await Responses.DogmeatAnswersAsync();
                Vars.Memes = await Responses.DogmeatMemesAsync();
                Vars.RawResponses = await Responses.DogmeatResponsesAsync();
                
                ConfigManager.SaveConfig();
                
                using (HttpClient Client = new HttpClient())
                {
                    HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/latestcommit.txt");
                    Vars.LatestCommit = await Response.Content.ReadAsStringAsync();
                }
                
                Logger.Log("Variables Updated");
                Thread.Sleep(600000);
            }
            Task.Delay(-1);
        }
    }
}