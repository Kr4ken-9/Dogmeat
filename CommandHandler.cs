using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DogMeat   
{
    public class CommandHandler
    {
        public static async Task Initialize()
        {
            await Vars.CService.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly());

            Vars.Client.MessageReceived += HandleCommand;
        }

        public static async Task HandleCommand(SocketMessage CommandParameter)
        {
            SocketUserMessage Message = CommandParameter as SocketUserMessage;

            if (Message == null) return;

            int argPos = 0;

            if (!(Message.HasMentionPrefix(Vars.Client.CurrentUser, ref argPos) || Message.HasCharPrefix('~', ref argPos)) || Message.HasStringPrefix("~~", ref argPos)) return;

            CommandContext Context = new CommandContext(Vars.Client, Message);

            IResult Result = await Vars.CService.ExecuteAsync(Context, argPos);

            if (!Result.IsSuccess)
                await Message.Channel.SendMessageAsync($"**Error:** {Result.ErrorReason}");
            else
                Utilities.Log("Executed a command.", (Message.Channel as SocketGuildChannel).Guild, Message.Author);
        }
    }
}