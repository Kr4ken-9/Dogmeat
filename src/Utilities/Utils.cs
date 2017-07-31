using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Discord;
using Discord.WebSocket;

namespace Dogmeat.Utilities
{
    public class Utils
    {
        #region Variables

        public static bool ContinueShutdown;

        public static IRole GetMasterRole(SocketGuild Guild) => Guild.Roles.FirstOrDefault(role => role.Name == "Master");

        public static IRole GetMutedRole(SocketGuild Guild) => Guild.Roles.FirstOrDefault(role => role.Name == "Muted");

        public static IRole GetRole(SocketGuild Guild, ulong ID) => Guild.Roles.FirstOrDefault(role => role.Id == ID);

        public static IRole GetRole(SocketGuild Guild, String Name) => Guild.Roles.FirstOrDefault(role => role.Name == Name);

        private static async Task<String[]> DogmeatResponsesAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/replies.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatMemesAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/memes.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatAnswersAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/answers.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String> ResponsePickerAsync(String Content)
        {
            String[] Responses = Vars.Responses;
            Random r = new Random();

            if (Content.Contains("MASTER") ||
                Content.Contains("CREATOR") ||
                Content.Contains("DEVELOPER"))
                return "*I am reluctantly groomed by that faggot Kr4ken*";

            else if (Content.Contains("?"))
            {
                String[] Answers = Vars.Answers;
                return Answers[new Random().Next(0, Answers.Length)];
            }

            #region Mexican

            else if (Content.Contains("JUAN") ||
                Content.Contains("MEXICO") ||
                Content.Contains("MEXICAN") ||
                Content.Contains("EDUARDO") ||
                Content.Contains("TRUMP") ||
                Content.Contains("DONALD") ||
                Content.Contains("PRESIDENT"))
                return Responses[r.Next(19, 21)];

            #endregion Mexican

            #region Jew

            else if (Content.Contains("JEW"))
                return Responses[r.Next(16, 19)];

            #endregion Jew

            #region AA

            else if (Content.Contains("BLACK") ||
                Content.Contains("NIGGER"))
                return Responses[15];

            #endregion AA

            #region Clinton

            else if (Content.Contains("HILLARY") ||
                Content.Contains("CLINTON") ||
                Content.Contains("MEME QUEEN"))
                return Responses[9];

            #endregion Clinton

            #region Insult

            else if (Content.Contains("FUCK") ||
                Content.Contains("CUNT") ||
                Content.Contains("ASSHOLE") ||
                Content.Contains("DOUCHE") ||
                Content.Contains("KYS") ||
                Content.Contains("ROAST") ||
                Content.Contains("COCK"))
                return Responses[4];

            #endregion Insult

            #region LGBT

            else if (Content.Contains("GAY") ||
                Content.Contains("LESBIAN") ||
                Content.Contains("TRANS") ||
                Content.Contains("SEXUAL"))
                return Responses[5];

            #endregion LGBT

            #region Arya

            else if (Content.Contains("RACIS") ||
                Content.Contains("HITLER") ||
                Content.Contains("RACE"))
                return Responses[6];

            #endregion Arya

            #region Female

            else if (Content.Contains("WOMEN") ||
                Content.Contains("GIRL") ||
                Content.Contains("WOMAN") ||
                Content.Contains("GRILL") ||
                Content.Contains("VAGINA") ||
                Content.Contains("PUSSY"))
                return Responses[7];

            #endregion Female

            else
                return Responses[r.Next(0, Responses.Length)];
        }

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

        public static async Task MaintainConnection()
        {
            while (Vars.KeepAlive)
            {
                if (Vars.Client.ConnectionState == ConnectionState.Disconnected)
                {
                    Vars.Client.LoginAsync(TokenType.Bot, Vars.Token);
                    Vars.Client.StartAsync();

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

        public static void Shutdown()
        {
            Vars.KeepAlive = false;
            ContinueShutdown = true;
            Logger.Log("Client is shutting down in five seconds.", ConsoleColor.Red);
            new Thread(() =>
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

                    Disconnect();
                    Environment.Exit(0);
                }
            }).Start();
        }

        public static void Disconnect()
        {
            Vars.KeepAlive = false;
            Vars.Client.StopAsync();
            Logger.Log("Dogmeat disconnected.", ConsoleColor.Red);
        }
        
        #endregion

        public static async Task UpdateVars()
        {
            while (Vars.KeepAlive)
            {
                Vars.Answers = DogmeatAnswersAsync().Result;
                Vars.Memes = DogmeatMemesAsync().Result;
                Vars.Responses = DogmeatResponsesAsync().Result;
                
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

        public static async Task CreateMutedRole(SocketGuild Guild)
        {
            await Guild.CreateRoleAsync("Muted", null, Color.Red);
            
            foreach (SocketTextChannel Channel in Guild.TextChannels)
                Channel.AddPermissionOverwriteAsync(GetMutedRole(Guild), Vars.MutedPermissions);
        }
    }
}