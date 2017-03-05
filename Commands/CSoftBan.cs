using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DogMeat;

namespace DogMeat.Commands
{
    public class CSoftBan : ModuleBase
    {
        [Command("softban"), Summary("Softbans a player by id or name")]
        public async Task SoftBanAsync([Summary("Id or name of person to softban")] string name)
        {
            if (Utilities.GetMasterRole((SocketGuild)Context.Guild) == null)
                await ReplyAsync("I have no master on this server.");
            else if (!((SocketGuildUser)Context.User).RoleIds.Contains(Utilities.GetMasterRole((SocketGuild)Context.Guild).Id))
                await ReplyAsync("You must be my master to execute this command.");
            else if (Context.Message.MentionedUserIds.Count != 0 && Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault()) != null)
            {
                await Context.Guild.AddBanAsync(Context.Message.MentionedUserIds.FirstOrDefault());
                await ReplyAsync(name + " is no more.");
            }
            else if (await Utilities.GetUser(Context.Guild, name) != null)
            {
                Context.Guild.AddBanAsync(await Utilities.GetUser(Context.Guild, name));
                await ReplyAsync(name + " is no more.");
            }
            else
                await ReplyAsync("Who the fuck is " + name + "?");
        }
    }
}
