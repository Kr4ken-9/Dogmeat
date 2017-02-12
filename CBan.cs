using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace DogMeat
{
    public class CBan : ModuleBase
    {
        [Command("ban"), Summary("Bans a player by id or name")]
        public async Task BanAsync([Summary("Id or name of person to ban")] string name)
        {
            if (Utilities.GetMasterRole((SocketGuild)Context.Guild) == null)
            {
                await Context.Channel.SendMessageAsync("I have no master on this server.");
                return;
            }
            else if (!((SocketGuildUser)Context.User).RoleIds.Contains(Utilities.GetMasterRole((SocketGuild)Context.Guild).Id))
            {
                await Context.Channel.SendMessageAsync("You must be my master to execute this command.");
                return;
            }
            else if (Context.Message.MentionedUserIds.Count != 0 && Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault()) != null)
            {
                await Context.Guild.AddBanAsync(Context.Message.MentionedUserIds.FirstOrDefault());
                ReplyAsync(name + " is no more.");
            }
            else if (ulong.TryParse(name, out ulong result))
            {
                if (await Context.Guild.GetUserAsync(result) != null)
                {
                    Context.Guild.AddBanAsync(await Context.Guild.GetUserAsync(result));
                    ReplyAsync(name + " is no more.");
                }
            }
            else
            {
                var users = await Context.Guild.GetUsersAsync();
                foreach (var user in users)
                {
                    if (user.Username == name || user.Nickname == name)
                    {
                        Context.Guild.AddBanAsync(user);
                        ReplyAsync(name + " is no more.");
                        return;
                    }
                }
                await ReplyAsync("Who the fuck is " + name + "?");
            }
        }
    }
}
