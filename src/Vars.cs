using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Database;
using SteamWebAPI2.Interfaces;

namespace Dogmeat
{
    public static class Vars
    {
        private static Random random;

        public static String SteamAPIKey = "", LatestCommit;

        private static IChannel commands;

        private static String[] answers;

        private static bool underMaintenance = true;

        private static DatabaseHandler dbHandler;

        private static SteamUser steamInterface;

        private static IGuild main;

        private static DiscordSocketClient client;

        private static CommandService cService;

        private static OverwritePermissions mutedChannelPermissions =
            new OverwritePermissions(sendMessages: PermValue.Deny);

        private static GuildPermissions mutedPermissions = new GuildPermissions(sendMessages: false);
        private static string token;
        private static IChannel logging;
        private static string[] rawResponses;
        private static string[] memes;
        private static bool keepAlive;

        // YOU ASKED FOR THIS KENNETH

        public static Random Random { get => random; set => random = value; }
        public static string Token { get => token; set => token = value; }
        public static IChannel Logging { get => logging; set => logging = value; }
        public static IChannel Commands { get => commands; set => commands = value; }
        public static string[] RawResponses { get => rawResponses; set => rawResponses = value; }
        public static string[] Memes { get => memes; set => memes = value; }
        public static string[] Answers { get => answers; set => answers = value; }
        public static bool KeepAlive { get => keepAlive; set => keepAlive = value; }
        public static bool UnderMaintenance { get => underMaintenance; set => underMaintenance = value; }
        public static DatabaseHandler DBHandler { get => dbHandler; set => dbHandler = value; }
        public static SteamUser SteamInterface { get => steamInterface; set => steamInterface = value; }
        public static IGuild Main { get => main; set => main = value; }
        public static DiscordSocketClient Client { get => client; set => client = value; }
        public static CommandService CService { get => cService; set => cService = value; }
        public static OverwritePermissions MutedChannelPermissions { get => mutedChannelPermissions; set => mutedChannelPermissions = value; }
        public static GuildPermissions MutedPermissions { get => mutedPermissions; set => mutedPermissions = value; }

        // Make sure this is synchronized with database time zone
        public static DateTime Now() =>
            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "EST");
    }
}
