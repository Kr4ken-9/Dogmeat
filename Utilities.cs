using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Discord;
using Discord.WebSocket;

namespace DogMeat
{
    public class Utilities
    {
        #region Variables

        public static bool ContinueShutdown;

        public static IRole GetMasterRole(SocketGuild Guild)
        {
            SocketRole Role = Guild.Roles.FirstOrDefault(role => role.Name == "Master");
            
            return Role ?? null;
        }

        public static IRole GetMutedRole(SocketGuild Guild)
        {
            SocketRole Role = Guild.Roles.FirstOrDefault(role => role.Name == "Muted");

            return Role ?? null;
        }

        public static IRole GetRole(SocketGuild Guild, ulong ID)
        {
            SocketRole Role = Guild.Roles.FirstOrDefault(role => role.Id == ID);

            return Role ?? null;
        }

        public static IRole GetRole(SocketGuild Guild, String Name)
        {
            SocketRole Role = Guild.Roles.FirstOrDefault(role => role.Name == Name);

            return Role ?? null;
        }

        private static async Task<String[]> DogmeatResponsesAsync()
        {
            HttpClient Client = new HttpClient();
            String url = "http://198.245.61.226/kr4ken/dogmeat_replies.txt";
            HttpResponseMessage Response = await Client.GetAsync(url);
            string Content = await Response.Content.ReadAsStringAsync();
            return Content.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatMemesAsync()
        {
            HttpClient Client = new HttpClient();
            String url = "http://198.245.61.226/kr4ken/dogmeat_memes.txt";
            HttpResponseMessage Response = await Client.GetAsync(url);
            string Content = await Response.Content.ReadAsStringAsync();
            return Content.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatAnswersAsync()
        {
            HttpClient Client = new HttpClient();
            String url = "http://198.245.61.226/kr4ken/dogmeat_answers.txt";
            HttpResponseMessage Response = await Client.GetAsync(url);
            string Content = await Response.Content.ReadAsStringAsync();
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

        #region Logging

        public static void Log(String Message)
        {
            Log(Message, ConsoleColor.Gray);
        }

        public static void Log(String Message, ConsoleColor Color)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(DateTime.Now + ": " + Message);
            Console.ResetColor();
            ((SocketTextChannel)Vars.Logging).SendMessageAsync(DateTime.Now + ": " + Message);
        }

        public static void Log(String Message, SocketGuild Guild, SocketUser User)
        {
            Log(Message, ConsoleColor.Gray, Guild, User);
        }

        public static void Log(String Message, ConsoleColor Color, SocketGuild Guild, SocketUser User)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(DateTime.Now + ": [" + Guild.Name + "] " + User.Username + " " + Message);
            Console.ResetColor();
            ((SocketTextChannel)Vars.Logging).SendMessageAsync(DateTime.Now + ": [" + Guild.Name + "] " + User.Username + " " + Message);
        }

        #endregion Logging

        #region Users

        private static async Task<IGuildUser> GetUserByName(IGuild Guild, String Name)
        {
            IReadOnlyCollection<IGuildUser> users = await Guild.GetUsersAsync();
            foreach (IGuildUser user in users)
            {
                if (user.Username.Contains(Name))
                    return user;
                else if (user.Nickname != null && user.Nickname.Contains(Name))
                    return user;
            }
            return null;
        }

        private static async Task<IGuildUser> GetUserByID(IGuild Guild, String ID)
        {
            if (ulong.TryParse(ID, out ulong result))
                if (await Guild.GetUserAsync(result) != null)
                    return await Guild.GetUserAsync(result);
            return null;
        }

        public static async Task<IGuildUser> GetUser(IGuild Guild, String Input)
        {
            IGuildUser nameResult = await GetUserByName(Guild, Input);
            IGuildUser IDResult = await GetUserByID(Guild, Input);
            if (nameResult != null)
                return nameResult;
            else if (IDResult != null)
                return IDResult;
            else
                return null;
        }

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
            Log("Client is shutting down in five seconds.", ConsoleColor.Red);
            new Thread(() =>
            {
                if (!ContinueShutdown) return;
                Log("5", ConsoleColor.Red);
                Thread.Sleep(1000);
                if (!ContinueShutdown) return;
                Log("4", ConsoleColor.Red);
                Thread.Sleep(1000);
                if (!ContinueShutdown) return;
                Log("3", ConsoleColor.Red);
                Thread.Sleep(1000);
                if (!ContinueShutdown) return;
                Log("2", ConsoleColor.Red);
                Thread.Sleep(1000);
                if (!ContinueShutdown) return;
                Log("1", ConsoleColor.Red);
                Thread.Sleep(1000);
                if (!ContinueShutdown) return;
                Disconnect();
                Environment.Exit(0);
            }).Start();
        }

        public static void Disconnect()
        {
            Vars.KeepAlive = false;
            Vars.Client.StopAsync();
            Log("Dogmeat disconnected.", ConsoleColor.Red);
        }
        
        #endregion

        public static async Task UpdateVars()
        {
            while (Vars.KeepAlive)
            {
                Vars.Answers = DogmeatAnswersAsync().Result;
                Vars.Memes = DogmeatMemesAsync().Result;
                Vars.Responses = DogmeatResponsesAsync().Result;
                Log("Variables Updated");
                Thread.Sleep(600000);
            }
            Task.Delay(-1);
        }
    }
}