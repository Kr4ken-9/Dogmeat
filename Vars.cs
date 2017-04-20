using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace DogMeat
{
    public class Vars
    {
        public static IChannel Logging;

        public static IChannel Commands;

        public static IGuild ManPAD;

        public static IGuild Main;

        public static DiscordSocketClient Client;

        public static bool KeepAlive = true;
    }
}
