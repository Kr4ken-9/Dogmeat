using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Discord.Commands;
using Discord;

namespace DogMeat.Commands
{ 
    public class CPurge : ModuleBase
    {
        [Command("purge"), Summary("Purges messages on executed channel.")]
        public async Task Purge([Summary("Number of messages to purge")] int count)
        {
            Context.Channel.DeleteMessagesAsync(await Context.Channel.GetMessagesAsync(count).Flatten());
            Context.Channel.SendMessageAsync("Purged " + count + " messages.");
            IEnumerable<IMessage> message = Context.Channel.GetMessagesAsync(1).ToEnumerable().FirstOrDefault();
            Task.Delay(5000);
            Context.Channel.DeleteMessagesAsync(message);
        }
    }
}
