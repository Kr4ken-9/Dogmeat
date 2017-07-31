using System;
using Discord.WebSocket;

namespace Dogmeat.Utilities
{
    public class Logger
    {
        public static void Log(String Message) => Log(Message, ConsoleColor.Gray);

        public static void Log(String Message, ConsoleColor Color)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(DateTime.Now + ": " + Message);
            Console.ResetColor();
            ((SocketTextChannel)Vars.Logging).SendMessageAsync(DateTime.Now + ": " + Message);
        }

        public static void Log(String Message, SocketGuild Guild, SocketUser User) => Log(Message, ConsoleColor.Gray, Guild, User);

        public static void Log(String Message, ConsoleColor Color, SocketGuild Guild, SocketUser User)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(DateTime.Now + ": [" + Guild.Name + "] " + User.Username + " " + Message);
            Console.ResetColor();
            ((SocketTextChannel)Vars.Logging).SendMessageAsync(DateTime.Now + ": [" + Guild.Name + "] " + User.Username + " " + Message);
        }
    }
}