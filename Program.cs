﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Net.Http;

namespace DogMeat
{
    class Program
    {
        private static DiscordSocketClient Client;

        private CommandHandler Handler;

        private SocketGuild ManPAD;

        private SocketGuild Log;

        public static bool KeepAlive = true;

        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Client = new DiscordSocketClient();

            Client.MessageReceived += async (msg) =>
            {
                if (msg.Channel.Id == 242948289404600321 && msg.Content.ToUpper() != "MANPAD SOUNDS LIKE A FEMININE CLEANING PRODUCT")
                    await WrongChannelAsync(msg);
                else if (msg.Channel.Id == 242948289404600321 && msg.Content.ToUpper() == "MANPAD SOUNDS LIKE A FEMININE CLEANING PRODUCT")
                    await AccessAsync(msg);
                else if (msg.Content.ToUpper().Contains("DOGMEAT") && !msg.Author.IsBot)
                    await MentionedAsync(msg);
            };

            await Client.LoginAsync(TokenType.Bot, "");
            await Client.ConnectAsync();

            Thread Connection = new Thread(() => Utilities.MaintainConnection(Client));
            Connection.Start();

            Utilities.AwaitInput(Client);

            #region Commands
            DependencyMap Map = new DependencyMap();
            Map.Add(Client);

            Handler = new CommandHandler();
            await Handler.Initialize(Map);
            #endregion Commands

            ManPAD = Client.GetGuild(242946566296436739);
            Log = Client.GetGuild(272850920059174914);

            await Task.Delay(-1);
        }

        public async Task AccessAsync(SocketMessage e)
        {
            await e.DeleteAsync();
            await (e.Author as SocketGuildUser).AddRolesAsync(ManPAD.GetRole(272789680821370881));
            Console.WriteLine("[" + (e.Channel as SocketGuildChannel).Guild.Name + "] " + e.Author.Username + " underwent initiation.");
        }

        public async Task WrongChannelAsync(SocketMessage e)
        {
            await e.DeleteAsync();
            Discord.Rest.RestDMChannel channel = await e.Author.CreateDMChannelAsync();
            await channel.SendMessageAsync("You are not permitted to chat in that channel.");
            Console.WriteLine("[" + (e.Channel as SocketGuildChannel).Guild.Name + "] " + e.Author.Username + " attempted to chat in a restricted channel.");
        }

        public async Task MentionedAsync(SocketMessage e)
        {
            await e.Channel.SendMessageAsync(await Utilities.ResponsePicker_Async(e.Content.ToUpper()));
            Console.WriteLine("[" + (e.Channel as SocketGuildChannel).Guild.Name + "] " + e.Author.Username + " mentioned me.");
        }
    }
}