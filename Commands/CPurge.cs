using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

namespace DogMeat.Commands
{ 
    public class CPurge : ModuleBase
    {
        [Command("purge"), Summary("Purges messages on executed channel.")]
        public async Task Purge([Summary("Number of messages to purge")] int count)
        {
            if (Utilities.GetMasterRole((SocketGuild) Context.Guild) == null)
            {
                await ReplyAsync("I have no master on this server.");
                return;
            }
            else if (!((SocketGuildUser) Context.User).Roles.Contains(Utilities.GetMasterRole((SocketGuild) Context.Guild)))
            {
                await ReplyAsync("You must be my master to execute this command.");
                return;
            }

            Context.Channel.DeleteMessagesAsync(await Context.Channel.GetMessagesAsync(count).Flatten());
            Context.Channel.SendMessageAsync("Purged " + count + " messages.");
        }
    }
}
