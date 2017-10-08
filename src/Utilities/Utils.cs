using System;
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

        private static async Task<String[]> DogmeatResponsesAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/replies.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatMemesAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/memes.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatAnswersAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/answers.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String> ResponsePickerAsync(String Content)
        {
            String[] Responses = Vars.Responses;

            if (Content.Contains("MASTER") ||
                Content.Contains("CREATOR") ||
                Content.Contains("DEVELOPER"))
                return "*I am reluctantly groomed by that faggot Kr4ken*";

            if (Content.Contains("?"))
            {
                String[] Answers = Vars.Answers;
                return Answers[Vars.Random.Next(0, Answers.Length)];
            }

            #region Mexican

            if (Content.Contains("JUAN") ||
                Content.Contains("MEXICO") ||
                Content.Contains("MEXICAN") ||
                Content.Contains("EDUARDO") ||
                Content.Contains("TRUMP") ||
                Content.Contains("DONALD") ||
                Content.Contains("PRESIDENT"))
                return Responses[Vars.Random.Next(19, 21)];

            #endregion

            #region Jew

            if (Content.Contains("JEW"))
                return Responses[Vars.Random.Next(16, 19)];

            #endregion

            #region AA

            if (Content.Contains("BLACK") ||
                Content.Contains("NIGG"))
                return Responses[15];

            #endregion

            #region Clinton

            if (Content.Contains("HILLARY") ||
                Content.Contains("CLINTON") ||
                Content.Contains("MEME QUEEN"))
                return Responses[9];

            #endregion

            #region Insult

            if (Content.Contains("FUCK") ||
                Content.Contains("CUNT") ||
                Content.Contains("ASSHOLE") ||
                Content.Contains("DOUCHE") ||
                Content.Contains("KYS") ||
                Content.Contains("ROAST") ||
                Content.Contains("COCK"))
                return Responses[4];

            #endregion

            #region LGBT

            if (Content.Contains("GAY") ||
                Content.Contains("LESBIAN") ||
                Content.Contains("TRANS") ||
                Content.Contains("SEXUAL"))
                return Responses[5];

            #endregion

            #region Arya

            if (Content.Contains("RACIS") ||
                Content.Contains("HITLER") ||
                Content.Contains("RACE"))
                return Responses[6];

            #endregion

            #region Female

            if (Content.Contains("WOMEN") ||
                Content.Contains("GIRL") ||
                Content.Contains("WOMAN") ||
                Content.Contains("GRILL") ||
                Content.Contains("VAGINA") ||
                Content.Contains("PUSSY"))
                return Responses[7];

            #endregion
            
            return Responses[Vars.Random.Next(0, Responses.Length)];
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
                Vars.Answers = await DogmeatAnswersAsync();
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

        public static async Task<IRole> CreateMutedRole(IGuild Guild)
        {
            IRole Muted = await Guild.CreateRoleAsync("Muted", Vars.MutedPermissions, Color.Red);

            foreach (SocketTextChannel Channel in ((SocketGuild) Guild).TextChannels)
                Channel.AddPermissionOverwriteAsync(Muted, Vars.MutedChannelPermissions);

            return Muted;
        }

        public static async Task<EMaster> CheckMasterAsync(IGuild Guild, IUser User)
        {
            if (GetMasterRole(Guild) == null)
                return EMaster.NONE;

            return !((SocketGuildUser) User).Roles.Contains(GetMasterRole(Guild))
                ? EMaster.FALSE
                : EMaster.TRUE;
        }
    }
}