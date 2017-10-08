using System;
using System.Collections.Generic;
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

        public static IGuild PointBlank;

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
        
        public static Dictionary<String, int[]> TailoredResponses = new Dictionary<String, int[]>
        {
            #region Mexican
            { "JUAN", new[] { 19, 20 } },
            { "MEXICO", new[] { 19, 20 } },
            { "EDUARDO", new[] { 19, 20 } },
            { "TRUMP", new[] { 19, 20 } },
            { "DONALD", new[] { 19, 20 } },
            { "PRESIDENT", new[] { 19, 20 } },
            #endregion
            #region Jew
            { "JEW", new[] { 16, 17, 18 } },
            #endregion
            #region AA
            { "BLACK", new[] { 15 } },
            { "NIGG", new[] { 15 } },
            #endregion
            #region Hillary
            { "HILLARY", new[] { 19 } },
            { "CLINTON", new[] { 19 } },
            { "MEME QUEEN", new [] { 19 } },
            #endregion
            #region Insult
            { "FUCK", new[] { 4 } },
            { "CUNT", new[] { 4 } },
            { "ASS", new[] { 4 } },
            { "DOUCHE", new[] { 4 } },
            { "KYS", new[] { 4 } },
            { "DIE", new[] { 4 } },
            { "ROAST", new[] { 4 } },
            { "COCK", new[] { 4 } },
            #endregion
            #region LGBT
            { "GAY", new[] { 5 } },
            { "LESBIAN", new[] { 5 } },
            { "TRANS", new[] { 5 } },
            { "SEXUAL", new[] { 5 } },
            #endregion
            #region Supremacy
            { "RACIS", new[] { 6 } },
            { "HITLER", new[] { 6 } },
            { "RACE", new[] { 6 } },
            { "ARYA", new[] { 6 } },
            #endregion
            #region Female
            { "WOMEN", new[] { 7 } },
            { "GIRL", new[] { 7 } },
            { "WOMAN", new[] { 7 } },
            { "GRILL", new[] { 7 } },
            { "VAGINA", new[] { 7 } },
            { "PUSSY", new[] { 7 } },
            #endregion
        };
    }
}
