using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DogMeat
{
    public class Vars
    {
        public static string Token = "";

        public static IChannel Logging;

        public static IChannel Commands;

        public static IGuild PointBlank;

        public static IGuild Main;

        public static DiscordSocketClient Client;

        public static bool KeepAlive = true;

        public static CommandHandler CHandler;

        public static CommandService CService;

        public static IServiceProvider ISProvider;
    }
}
