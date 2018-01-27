using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Config;
using Dogmeat.Utilities;
using Dogmeat.Database;
using Newtonsoft.Json;
using SteamWebAPI2.Interfaces;

namespace Dogmeat
{
    class Program
    {
        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Vars.Random = new Random();
            Vars.Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                WebSocketProvider = Discord.Net.Providers.WS4Net.WS4NetProvider.Instance
            });
            Vars.CService = new CommandService();

            ConfigManager.LoadConfig();

            await CheckVariables();
            
            DatabaseHandler.LoadConnection();
            await Vars.Client.LoginAsync(TokenType.Bot, Vars.Token);
            await Vars.Client.StartAsync();

            Vars.Client.Log += async msg => Console.WriteLine(msg.Message);

            Vars.Client.Ready += OnStart;

            await Task.Delay(-1);
        }

        private async Task OnStart()
        {
            Vars.Main = Vars.Client.GetGuild(281249097770598402);
            Vars.Commands = await Vars.Main.GetChannelAsync(297587358063394816);
            Vars.Logging = await Vars.Main.GetChannelAsync(297587378804097025);
            Vars.SteamInterface = new SteamUser(Vars.SteamAPIKey);

            MessageHandler.InitializeListener();
            await Vars.CService.AddModulesAsync(Assembly.GetEntryAssembly());

            using (DatabaseHandler Context = new DatabaseHandler())
            {
                await Context.Database.EnsureCreatedAsync();

                Context.UUIHandler.ExpHandler.ExperienceUpdate += async (sender, args) =>
                    await Context.UUIHandler.ExpHandler.IncreaseExperience(args.User.ID, args.Amount, args.Context);
            }
            
            CancellationToken Token = new CancellationTokenSource().Token;

            new Task(() => Misc.UpdateVarsAsync(), Token, TaskCreationOptions.LongRunning).Start();

            if (Vars.UnderMaintenance)
                Vars.Client.SetGameAsync("Under Maintenace");
        }

        private async Task CheckVariables()
        {
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

            if (!ConfigManager.CheckConfigItem("mysql.json"))
            {
                Console.WriteLine("You must have a mysql configuration for database functionality.");
                Console.WriteLine("Would you like to configure it now? Y/N");

                Connection PotentialConnection = DatabaseHandler.AggregateConnection(Console.ReadLine());

                if (PotentialConnection != null)
                    File.WriteAllText(ConfigManager.ConfigPath("mysql.json"),
                        JsonConvert.SerializeObject(PotentialConnection, Formatting.Indented));
            }
        }

    }
}