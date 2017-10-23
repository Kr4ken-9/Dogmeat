using System;
using Discord.WebSocket;

namespace Dogmeat.Utilities
{
    public class Logger
    {
        public static void Log(String Message, ConsoleColor Color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(DateTime.Now + ": " + Message);
            Console.ResetColor();
            ((SocketTextChannel)Vars.Logging).SendMessageAsync(DateTime.Now + ": " + Message);
        }

        public static void Log(String Message, SocketGuild Guild, SocketUser User, ConsoleColor Color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(DateTime.Now + ": [" + Guild.Name + "] " + User.Username + " " + Message);
            Console.ResetColor();
            ((SocketTextChannel)Vars.Logging).SendMessageAsync(DateTime.Now + ": [" + Guild.Name + "] " + User.Username + " " + Message);
        }
    }
}