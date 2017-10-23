using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Dogmeat.Config;
using Dogmeat.Utilities;

namespace Dogmeat   
{
    public class MessageHandler
    {

        #region Owner Commands

        public static async Task InitializeOwnerCommandsHandler() => Vars.Client.MessageReceived += async msg =>
        {
            if (msg.Channel.Id == Vars.Commands.Id && !msg.Content.Contains("~") &&
                msg.Author.Id != Vars.Client.CurrentUser.Id)
                await HandleOwnerCommand(msg);
        };

        private static async Task HandleOwnerCommand(SocketMessage msg)
        {
            String Input = msg.Content;

            if (Input == null)
                return;

            String[] Inputs = Input.Split('"')
                .Select((element, index) => index % 2 == 0
                    ? element.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                    : new[] {element})
                .SelectMany(element => element).ToArray();

            OwnerCommand(Inputs);
            
            Logger.Log("Issued command " + Inputs[0], Vars.Main as SocketGuild, msg.Author);
        }

        private static async Task OwnerCommand(String[] Parameters)
        {
            switch (Parameters[0].ToUpperInvariant())
            {
                case "ANNOUNCE":
                case "SAY":
                    if (!ulong.TryParse(Parameters[1], out ulong ID))
                        Logger.Log("That is not a valid ID.");

                    String output = Parameters[2];

                    SocketTextChannel Channel = (SocketTextChannel)Vars.Client.GetChannel(ID);

                    Channel?.SendMessageAsync(output);
                    break;
                case "SHUTDOWN":
                case "QUIT":
                case "EXIT":
                    Misc.ShutdownAsync();
                    break;
                case "DISCONNECT":
                    Misc.DisconnectAsync();
                    break;
                case "RECONNECT":
                    Vars.KeepAlive = true;
                    Logger.Log("Dogmeat was revived.", ConsoleColor.Green);
                    break;
                case "CANCEL":
                    Logger.Log("Shutdown canceled", ConsoleColor.Green);
                    Misc.ContinueShutdown = false;
                    break;
                case "SAVE":
                    ConfigManager.SaveConfig();
                    break;
                default:
                    Logger.Log(Parameters[0] + " is not a command.", ConsoleColor.Red);
                    break;
            }
        }

        #endregion
        
        #region General
        
        public static async Task InitializeGeneralHandler()
        {
            Vars.Client.MessageReceived += async msg =>
            {
                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(msg.Content, "Dogmeat",
                        CompareOptions.IgnoreCase) >= 0 && !msg.Author.IsBot)
                    await MentionedAsync(msg);
                
                else if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(msg.Content, "Taemgod",
                             CompareOptions.IgnoreCase) >= 0 && !msg.Author.IsBot)
                    await DefConAsync(msg);
                
                else if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(msg.Content, "Good boy",
                             CompareOptions.IgnoreCase) >= 0 && !msg.Author.IsBot)
                    await Patronization(msg);
            };
        }

        private static async Task MentionedAsync(SocketMessage e)
        {
            e.Channel.SendMessageAsync(await Responses.ResponsePickerAsync(e.Content.ToUpper()));
            Logger.Log("Mentioned me.", ((SocketGuildChannel)e.Channel).Guild, e.Author);
        }

        private static async Task DefConAsync(SocketMessage e)
        {
            e.Channel.SendMessageAsync("DefCon42 is a sexy beast. Also, fuck off.");
            Logger.Log("Reversed my name.", ((SocketGuildChannel)e.Channel).Guild, e.Author);
        }

        private static async Task Patronization(SocketMessage e)
        {
            e.Channel.SendMessageAsync("Don't patronize me, faggot.");
            Logger.Log("Patronized me.", ((SocketGuildChannel)e.Channel).Guild, e.Author);
        }
        
        #endregion
        
        #region Commands
        
        public static async Task InitializeCommandHandler()
        {
            await Vars.CService.AddModulesAsync(Assembly.GetEntryAssembly());

            Vars.Client.MessageReceived += HandleCommand;
        }

        private static async Task HandleCommand(SocketMessage CommandParameter)
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
                Logger.Log("Executed a command.", (Message.Channel as SocketGuildChannel).Guild, Message.Author);
        }
        
        #endregion
    }
}