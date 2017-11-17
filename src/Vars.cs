using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Database;
using SteamWebAPI2.Interfaces;

namespace Dogmeat
{
    public class Vars
    {
        public static Random Random;
        
        public static String Token = "", SteamAPIKey = "", LatestCommit;
        
        public static IChannel Logging, Commands;
        
        public static String[] RawResponses, Memes, Answers;
        
        public static bool KeepAlive = true, UnderMaintenance = true;

        public static DatabaseHandler DBHandler;

        public static SteamUser SteamInterface;

        public static IGuild Main;

        public static DiscordSocketClient Client;

        public static CommandService CService;

        public static OverwritePermissions MutedChannelPermissions =
            new OverwritePermissions(sendMessages: PermValue.Deny);

        public static GuildPermissions MutedPermissions = new GuildPermissions(sendMessages: false);

        // Make sure this is synchronized with database time zone
        public static DateTime Now() =>
            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "EST");
    }
}
