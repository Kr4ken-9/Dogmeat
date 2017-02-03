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
            while (true)
            {
                Thread.Sleep(1000);
                if (Client.ConnectionState == ConnectionState.Disconnected)
                {
                    Client.ConnectAsync();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(DateTime.Now + ": Dogmeat has disconnected and automagically reconnected.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(DateTime.Now + ": Client still connected.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Thread.Sleep(3599000);
            }
        }
    }
}