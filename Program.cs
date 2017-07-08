using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DogMeat
{
    class Program
    {
        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            Vars.Client = new DiscordSocketClient();
            Vars.CService = new CommandService();

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

            CommandHandler.Initialize();

            Vars.Client.MessageReceived += async (msg) =>
            {
                if (msg.Channel.Id == 333334079921455105)
                {
                    if (StringComparer.OrdinalIgnoreCase.Compare(msg.Content, "Klaatu barada nikto") == 0)
                        await Utilities.AccessAsync(msg);
                    else
                        await Utilities.WrongChannelAsync(msg);
                }
                else if (StringComparer.OrdinalIgnoreCase.Compare(msg.Content, "Dogmeat") == 0 && !msg.Author.IsBot)
                    await Utilities.MentionedAsync(msg);
            };

            new Thread(() => Utilities.MaintainConnection()).Start();

            Utilities.AwaitInput();
        }
    }
}