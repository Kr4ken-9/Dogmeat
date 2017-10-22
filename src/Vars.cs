using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.UUI;
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

        public static UserInfoHandler UUIHandler;

        public static SteamUser SteamInterface;

        public static IGuild Main;

        public static DiscordSocketClient Client;

        public static CommandService CService;

        public static IServiceProvider ISProvider;

        public static OverwritePermissions MutedChannelPermissions =
            new OverwritePermissions(sendMessages: PermValue.Deny);

        public static GuildPermissions MutedPermissions = new GuildPermissions(sendMessages: false);
    }
}
