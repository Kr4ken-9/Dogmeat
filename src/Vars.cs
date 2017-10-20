using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SteamWebAPI2.Interfaces;

namespace Dogmeat
{
    public class Vars
    {
        public static Random Random;
        
        public static String Token = "";

        public static String SteamAPIKey = "";

        public static SteamUser SteamInterface;

        public static IChannel Logging;

        public static IChannel Commands;

        public static IGuild Main;

        public static DiscordSocketClient Client;

        public static bool KeepAlive = true;

        public static CommandService CService;

        public static IServiceProvider ISProvider;

        public static String[] RawResponses;

        public static String[] Memes;

        public static String[] Answers;

        public static OverwritePermissions MutedChannelPermissions = new OverwritePermissions(PermValue.Inherit,
            PermValue.Inherit, PermValue.Inherit, PermValue.Inherit, PermValue.Deny);

        public static GuildPermissions MutedPermissions =
            new GuildPermissions(false, false, false, false, false, false, false, true, false);

        public static String LatestCommit;

        public static bool UnderMaintenance = true;
    }
}
