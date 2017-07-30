using System;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using SteamWebAPI2.Interfaces;

namespace DogMeat
{
    public class Vars
    {
        public static string Token = "";

        public static string SteamAPIKey = "";

        public static SteamUser SteamInterface;

        public static IChannel Logging;

        public static IChannel Commands;

        public static IGuild PointBlank;

        public static IGuild Main;

        public static DiscordSocketClient Client;

        public static bool KeepAlive = true;

        public static CommandService CService;

        public static IServiceProvider ISProvider;

        public static String[] Responses;

        public static String[] Memes;

        public static String[] Answers;
    }
}
