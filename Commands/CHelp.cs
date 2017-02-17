using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace DogMeat.Commands
{
    public class CHelp : ModuleBase
    {
        [Command("help"), Summary("Dogmeat is a free spirit, and only responds to exact circumstances.")]
        public async Task HelpAsync()
        {
            var DMChannel = await Context.User.CreateDMChannelAsync();
            await DMChannel.SendMessageAsync("As of " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version + " my commands include: " +
                "```~Ban - name/id\r\n" +
                "~Kick - name/id\r\n" +
                "~Info - No input\r\n" +
                "~Help - No input\r\n" +
                "~Lenny - No input\r\n" +
                "~Mute - name/id```\r\n" +
                "To execute administrative commands, you must first be in a role named 'Master'.\r\n" +
                "If I have no master, I will not execute administrative commands.\r\n" +
                "To execute the muted role, a role named 'Muted' must be available. This role must not have permission to type. (In the future I may automate this process).");
        }
    }
}
