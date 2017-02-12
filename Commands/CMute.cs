using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace DogMeat.Commands
{
    public class CMute : ModuleBase
    {
        [Command("mute"), Summary("Mutes the specified player")]
        public async Task Mute([Summary("ID or name of person to mute.")] string name)
        {
            if (Utilities.GetMasterRole((SocketGuild)Context.Guild) == null)
                await ReplyAsync("I have no master on this server.");
            else if (!((SocketGuildUser)Context.User).RoleIds.Contains(Utilities.GetMasterRole((SocketGuild)Context.Guild).Id))
                await ReplyAsync("You must be my master to execute this command.");
            else if (Utilities.GetMutedRole((SocketGuild)Context.Guild) == null)
                await ReplyAsync("I need a muted role to punish players with!");
            else if (Context.Message.MentionedUserIds.Count != 0 && Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault()) != null)
            {
                IGuildUser target = await Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault());
                SocketRole muted = Utilities.GetMutedRole((SocketGuild)Context.Guild);
                if (target.RoleIds.Contains(muted.Id))
                {
                    await target.RemoveRolesAsync(muted);
                    await ReplyAsync(target.Username + " is unmuted.");
                }
                else
                {
                    await target.AddRolesAsync(muted);
                    await ReplyAsync(target.Username + " is muted.");
                }
            }
            else if (await Utilities.GetUser(Context.Guild, name) != null)
            {
                SocketGuildUser target = await Utilities.GetUser(Context.Guild, name);
                SocketRole muted = Utilities.GetMutedRole((SocketGuild)Context.Guild);
                if (target.RoleIds.Contains(muted.Id))
                {
                    await target.RemoveRolesAsync(muted);
                    await ReplyAsync(target.Username + " is unmuted");
                }
                else
                {
                    await target.AddRolesAsync(muted);
                    await ReplyAsync(target.Username + " is muted.");
                }
            }
            else
                await ReplyAsync("Who the fuck is " + name + "?");
        }
    }
}
