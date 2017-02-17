using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace DogMeat
{
    public class Utilities
    {
        #region Variables

        private static bool ContinueShutdown;

        public static SocketRole GetMasterRole(SocketGuild Guild)
        {
            foreach (SocketRole Role in Guild.Roles)
            {
                if (Role.Name == "Master")
                    return Role;
            }
            return null;
        }

        public static SocketRole GetMutedRole(SocketGuild Guild)
        {
            foreach (SocketRole Role in Guild.Roles)
            {
                if (Role.Name == "Muted")
                    return Role;
            }
            return null;
        }

        private static async Task<String[]> DogmeatResponses_Async()
        {
            HttpClient Client = new HttpClient();
            String url = "http://198.245.61.226/kr4ken/dogmeat_replies.txt";
            HttpResponseMessage Response = await Client.GetAsync(url);
            string Content = await Response.Content.ReadAsStringAsync();
            return Content.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String> ResponsePicker_Async(String Content)
        {
            String[] Responses = await DogmeatResponses_Async();

            if (Content.Contains("JUAN") ||
                Content.Contains("MEXICO") ||
                Content.Contains("MEXICAN") ||
                Content.Contains("EDUARDO") ||
                Content.Contains("TRUMP") ||
                Content.Contains("DONALD") ||
                Content.Contains("PRESIDENT"))
                return Responses[10];

            else if (Content.Contains("HILLARY") ||
                Content.Contains("CLINTON") ||
                Content.Contains("MEME QUEEN"))
                return Responses[9];

            else if (Content.Contains("FUCK") ||
                Content.Contains("CUNT") ||
                Content.Contains("ASSHOLE") ||
                Content.Contains("DOUCHE") ||
                Content.Contains("KYS") ||
                Content.Contains("ROAST") ||
                Content.Contains("COCK"))
                return Responses[4];

            else if (Content.Contains("GAY") ||
                Content.Contains("LESBIAN") ||
                Content.Contains("TRANS") ||
                Content.Contains("SEXUAL"))
                return Responses[5];

            else if (Content.Contains("RACIS") ||
                Content.Contains("HITLER") ||
                Content.Contains("RACE"))
                return Responses[6];

            else if (Content.Contains("WOMEN") ||
                Content.Contains("GIRL") ||
                Content.Contains("WOMAN") ||
                Content.Contains("GRILL") ||
                Content.Contains("VAGINA") ||
                Content.Contains("PUSSY"))
                return Responses[7];

            else
            {
                Random r = new Random();
                return Responses[r.Next(0, Responses.Length)];
            }
        }

        #endregion Variables

        public static void MaintainConnection(DiscordSocketClient Client)
        {
            while (Program.KeepAlive)
            {
                Thread.Sleep(1000);
                if (Client.ConnectionState == ConnectionState.Disconnected)
                {
                    Client.LoginAsync(TokenType.Bot, "");
                    Client.ConnectAsync();
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
                Thread.Sleep(3599000);
            }
        }

        public static void AwaitInput(DiscordSocketClient Client)
        {
            while (true)
            {
                string Input = Console.ReadLine();
                switch (Input.ToUpperInvariant())
                {
                    case "SHUTDOWN":
                        Shutdown(Client);
                        break;
                    case "QUIT":
                        Shutdown(Client);
                        break;
                    case "EXIT":
                        Shutdown(Client);
                        break;
                    case "DISCONNECT":
                        Disconnect(Client);
                        break;
                    case "RECONNECT":
                        Program.KeepAlive = true;
                        Log("Dogmeat was revived.", ConsoleColor.Green);
                        break;
                    case "CANCEL":
                        Log("Shutdown canceled", ConsoleColor.Green);
                        ContinueShutdown = false;
                        break;
                    default:
                        Log("That is not a command.", ConsoleColor.Red);
                        break;
                }
            }
        }

        private static void Shutdown(DiscordSocketClient Client)
        {
            Program.KeepAlive = false;
            ContinueShutdown = true;
            Client.DisconnectAsync();
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
                Environment.Exit(0);
            }).Start();
        }

        private static void Disconnect(DiscordSocketClient Client)
        {
            Program.KeepAlive = false;
            Client.DisconnectAsync();
            Log("Dogmeat disconnected.", ConsoleColor.Red);
        }

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
    }
}