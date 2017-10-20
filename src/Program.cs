using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Config;
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
            Vars.Random = new Random();
            
            Vars.Client = new DiscordSocketClient();
            Vars.CService = new CommandService();
            
            ConfigManager.LoadConfig();

            while (String.IsNullOrEmpty(Vars.Token))
            {
                Console.WriteLine("Please enter Bot token:");
                Vars.Token = Console.ReadLine();
            }

            while (String.IsNullOrEmpty(Vars.SteamAPIKey))
            {
                Console.WriteLine("Please enter Steam API token:");
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
            Vars.Main = Vars.Client.GetGuild(281249097770598402);
            Vars.Commands = await Vars.Main.GetChannelAsync(297587358063394816);
            Vars.Logging = await Vars.Main.GetChannelAsync(297587378804097025);
            Vars.SteamInterface = new SteamUser(Vars.SteamAPIKey);

            MessageHandler.InitializeCommandHandler();

            MessageHandler.InitializeGeneralHandler();

            MessageHandler.InitializeOwnerCommandsHandler();
            
            #region Continous Tasks
            
            CancellationToken Token = new CancellationTokenSource().Token;
            
            new Task(() => Utils.MaintainConnectionAsync(), Token, TaskCreationOptions.LongRunning).Start();

            new Task(() => Utils.UpdateVarsAsync(), Token, TaskCreationOptions.LongRunning).Start();
            
            #endregion

            if (Vars.UnderMaintenance)
                Vars.Client.SetGameAsync("Under Maintenace");
        }

    }
}