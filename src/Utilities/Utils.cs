using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Dogmeat.Utilities
{
    public class Utils
    {
        #region Variables

        public static bool ContinueShutdown;

        public static IRole GetMasterRole(IGuild Guild) => Guild.Roles.FirstOrDefault(role => role.Name == "Master");

        public static IRole GetMutedRole(IGuild Guild) => Guild.Roles.FirstOrDefault(role => role.Name == "Muted");

        public static IRole GetRole(IGuild Guild, ulong ID) => Guild.Roles.FirstOrDefault(role => role.Id == ID);

        public static IRole GetRole(IGuild Guild, String Name) => Guild.Roles.FirstOrDefault(role => role.Name == Name);

        #endregion Variables

        #region Users

        public static async Task<int> GetAllUsers()
        {
            int Users = 0;

            foreach (SocketGuild Guild in Vars.Client.Guilds)
                Users += Guild.Users.Count;

            return Users;
        }

        private static async Task<IGuildUser> GetUserByName(IGuild Guild, String Name) =>
            (await Guild.GetUsersAsync()).FirstOrDefault(user => user.Nickname.Contains(Name) || user.Username.Contains(Name));

        private static async Task<IGuildUser> GetUserByID(IGuild Guild, String ID)
        {
            if (!ulong.TryParse(ID, out ulong Id))
                return null;

            return await Guild.GetUserAsync(Id);
        }

        public static async Task<IGuildUser> GetUser(IGuild Guild, String Input) =>
            await GetUserByName(Guild, Input) ?? await GetUserByName(Guild, Input);

        #endregion Users
        
        #region Connection

        public static async Task MaintainConnectionAsync()
        {
            while (Vars.KeepAlive)
            {
                if (Vars.Client.ConnectionState == ConnectionState.Disconnected)
                {
                    await DisconnectAsync();
                    
                    Vars.Client.LoginAsync(TokenType.Bot, Vars.Token);
                    Vars.Client.StartAsync();
                    Vars.KeepAlive = true;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(DateTime.Now + ": Dogmeat has disconnected and automagically reconnected.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(DateTime.Now + ": Client still connected.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Thread.Sleep(3600000);
            }
            Task.Delay(-1);
        }

        public static async Task ShutdownAsync()
        {
            Vars.KeepAlive = false;
            ContinueShutdown = true;
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
                Vars.Memes = Responses.DogmeatMemesAsync().Result;
                Vars.RawResponses = Responses.DogmeatResponsesAsync().Result;
                
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