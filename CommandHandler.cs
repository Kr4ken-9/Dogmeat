using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DogMeat   
{
    public class CommandHandler
    {
        private CommandService Commands;
        private DiscordSocketClient Client;
        private IDependencyMap Map;

        public async Task Initialize(IDependencyMap map)
        {
            Client = map.Get<DiscordSocketClient>();
            Commands = new CommandService();
            map.Add(Commands);
            Map = map;

            await Commands.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly());

            Client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage CommandParameter)
        {
            SocketUserMessage Message = CommandParameter as SocketUserMessage;
            if (Message == null) return;

            int argPos = 0;

            if (!(Message.HasMentionPrefix(Client.CurrentUser, ref argPos) || Message.HasCharPrefix('~', ref argPos))) return;

            CommandContext Context = new CommandContext(Client, Message);

            IResult Result = await Commands.ExecuteAsync(Context, argPos, Map);

            if (!Result.IsSuccess)
            {
                if (Result.Error != CommandError.UnknownCommand)
                    await Message.Channel.SendMessageAsync($"**Error:** {Result.ErrorReason}");
            }
            else
                Console.WriteLine("[" + (Message.Channel as SocketGuildChannel).Guild.Name + "] " + Message.Author.Username + " executed a command.");
        }
    }
}