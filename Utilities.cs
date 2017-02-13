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
        private static async Task<String[]> DogmeatResponses_Async()
        {
            HttpClient Client = new HttpClient();
            String url = "http://198.245.61.226/kr4ken/dogmeat_replies.txt";
            HttpResponseMessage Response = await Client.GetAsync(url);
            string Content = await Response.Content.ReadAsStringAsync();
            return Content.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
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
                if (Input.ToUpper() == "SHUTDOWN")
                {
                    Program.KeepAlive = false;
                    Client.DisconnectAsync();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(DateTime.Now + ": Dogmeat has been killed.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else if (Input.ToUpper() == "START")
                {
                    Client.LoginAsync(TokenType.Bot, "");
                    Client.ConnectAsync();
                    Program.KeepAlive = true;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(DateTime.Now + ": Dogmeat has been revived.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

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
            foreach(SocketRole Role in Guild.Roles)
            {
                if (Role.Name == "Muted")
                    return Role;
            }
            return null;
        }

        public static async Task<SocketGuildUser> GetUserByName(IGuild Guild, String Name)
        {
            IReadOnlyCollection<IGuildUser> users = await Guild.GetUsersAsync();
            foreach (IGuildUser user in users)
            {
                if (user.Username.Contains(Name))
                    return user as SocketGuildUser;
                else if (user.Nickname != null && user.Nickname.Contains(Name))
                    return user as SocketGuildUser;
            }
            return null;
        }

        public static async Task<SocketGuildUser> GetUserByID(IGuild Guild, String ID)
        {
            if (ulong.TryParse(ID, out ulong result))
                if (await Guild.GetUserAsync(result) != null)
                    return await Guild.GetUserAsync(result) as SocketGuildUser;
            return null;
        }

        public static async Task<SocketGuildUser> GetUser(IGuild Guild, String Input)
        {
            SocketGuildUser nameResult = await GetUserByName(Guild, Input);
            SocketGuildUser IDResult = await GetUserByID(Guild, Input);
            if (nameResult != null)
                return nameResult;
            else if (IDResult != null)
                return IDResult;
            else
                return null;
        }
    }
}