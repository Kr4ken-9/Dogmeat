using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Dogmeat.Utilities;
using Microsoft.Extensions.DependencyInjection;
using SteamWebAPI2.Interfaces;

namespace Dogmeat
{
    class Program
    {
        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Vars.Client = new DiscordSocketClient();
            Vars.CService = new CommandService();

            if (String.IsNullOrEmpty(Vars.Token))
            {
                Console.WriteLine("Please enter Bot token: ");
                Vars.Token = Console.ReadLine();
            }

            if (String.IsNullOrEmpty(Vars.SteamAPIKey))
            {
                Console.WriteLine("Please enter Steam API token: ");
                Vars.SteamAPIKey = Console.ReadLine();
            }

            await Vars.Client.LoginAsync(TokenType.Bot, Vars.Token);
            await Vars.Client.StartAsync();

            Vars.ISProvider = new ServiceCollection().BuildServiceProvider();

            Vars.Client.Ready += OnStart;

            await Task.Delay(-1);
        }

        private async Task OnStart()
        {
            Vars.PointBlank = Vars.Client.GetGuild(332435336921612299);
            Vars.Main = Vars.Client.GetGuild(281249097770598402);
            Vars.Commands = await Vars.Main.GetChannelAsync(297587358063394816);
            Vars.Logging = await Vars.Main.GetChannelAsync(297587378804097025);
            Vars.SteamInterface = new SteamUser(Vars.SteamAPIKey);

            MessageHandler.InitializeCommandHandler();

            MessageHandler.InitializeGeneralHandler();

            MessageHandler.InitializeOwnerCommandsHandler();
            
            #region Continous Tasks
            
            CancellationTokenSource Token = new CancellationTokenSource();
            
            new Task(() => Utils.MaintainConnection(), Token.Token, TaskCreationOptions.LongRunning).Start();

            new Task(() => Utils.UpdateVars(), Token.Token, TaskCreationOptions.LongRunning).Start();
            
            #endregion
        }

    }
}