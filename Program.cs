using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DogMeat
{
    class Program
    {
        #region Variables

        public static DiscordSocketClient Client;

        private CommandHandler Handler;

        #endregion Variables

        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Client = new DiscordSocketClient();

            Client.MessageReceived += async (msg) =>
            {
                if (msg.Channel.Id == 242948289404600321 && msg.Content.ToUpperInvariant() != "MANPAD SOUNDS LIKE A FEMININE CLEANING PRODUCT")
                    await Utilities.WrongChannelAsync(msg);
                else if (msg.Channel.Id == 242948289404600321 && msg.Content.ToUpperInvariant() == "MANPAD SOUNDS LIKE A FEMININE CLEANING PRODUCT")
                    await Utilities.AccessAsync(msg, (SocketGuild)Vars.ManPAD);
                else if (msg.Content.ToUpper().Contains("DOGMEAT") && !msg.Author.IsBot)
                    await Utilities.MentionedAsync(msg);
            };

            await Client.LoginAsync(TokenType.Bot, "");
            await Client.StartAsync();

            Client.Ready += OnStart;

            await Task.Delay(-1);
        }

        private async Task OnStart()
        {
            Vars.ManPAD = Client.GetGuild(242946566296436739);
            Vars.Main = Client.GetGuild(281249097770598402);
            Vars.Commands = await Vars.Main.GetChannelAsync(297587358063394816);
            Vars.Logging = await Vars.Main.GetChannelAsync(297587378804097025);

            #region Commands

            DependencyMap Map = new DependencyMap();
            Map.Add(Client);

            Handler = new CommandHandler();

            await Handler.Initialize(Map);

            #endregion Commands

            #region Initiation

            /*Initiation.CheckServerList(); Commented until working

            if (Initiation.GetServerList() != null)
                Initiation.Servers = Initiation.GetServerList();
            else
                Initiation.Servers = new List<Server>();

            Client.MessageReceived += Initiation.HandleInitiationAsync;*/

            #endregion Initiation

            Thread Connection = new Thread(() => Utilities.MaintainConnection());
            Connection.Start();

            Utilities.AwaitInput();
        }
    }
}