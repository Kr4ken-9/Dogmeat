using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{ 
    public class CPurge : ModuleBase
    {
        [Command("purge"), Alias("prune"), Summary("Purges messages on executed channel.")]
        public async Task Purge([Summary("Number of messages to purge")] int count, [Summary("User to prune")] IGuildUser User = null)
        {
            if (Utils.GetMasterRole((SocketGuild) Context.Guild) == null)
            {
                await ReplyAsync("I have no master on this server.");
                return;
            }
            else if (!((SocketGuildUser) Context.User).Roles.Contains(Utils.GetMasterRole((SocketGuild) Context.Guild)))
            {
                await ReplyAsync("You must be my master to execute this command.");
                return;
            }

            if (User == null)
            {
                Context.Channel.DeleteMessagesAsync(await Context.Channel.GetMessagesAsync(count).Flatten());
                Context.Channel.SendMessageAsync($"Purged {count} messages.");
            }
            else
            {
                IEnumerable<IMessage> Messages = await Context.Channel.GetMessagesAsync(count).Flatten();
                IEnumerable<IMessage> UserMessages = Messages.Where(Message => Message.Author == User);

                Context.Channel.DeleteMessagesAsync(UserMessages);
                Context.Channel.SendMessageAsync($"Purged {count} messages from {User.Nickname ?? User.Username}");
            }
        }
    }
}
