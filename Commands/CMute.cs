using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            else if (!((SocketGuildUser)Context.User).Roles.Contains(Utilities.GetMasterRole((SocketGuild)Context.Guild)))
                await ReplyAsync("You must be my master to execute this command.");
            else if (Utilities.GetMutedRole((SocketGuild)Context.Guild) == null)
                await ReplyAsync("I need a muted role to punish players with!");
            else if (Context.Message.MentionedUserIds.Count != 0 && Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault()) != null)
            {
                IGuildUser target = await Context.Guild.GetUserAsync(Context.Message.MentionedUserIds.FirstOrDefault());
                IRole muted = Utilities.GetMutedRole((SocketGuild)Context.Guild);
                if (target.RoleIds.Contains(muted.Id))
                {
                    await target.RemoveRoleAsync(muted);
                    await ReplyAsync(target.Username + " is unmuted.");
                }
                else
                {
                    await target.AddRoleAsync(muted);
                    await ReplyAsync(target.Username + " is muted.");
                }
            }
            else if (await Utilities.GetUser(Context.Guild, name) != null)
            {
                IGuildUser target = await Utilities.GetUser(Context.Guild, name);
                IRole muted = Utilities.GetMutedRole((SocketGuild)Context.Guild);
                if (target.RoleIds.Contains(muted.Id))
                {
                    await target.RemoveRoleAsync(muted);
                    await ReplyAsync(target.Username + " is unmuted");
                }
                else
                {
                    await target.AddRoleAsync(muted);
                    await ReplyAsync(target.Username + " is muted.");
                }
            }
            else
                await ReplyAsync("Who the fuck is " + name + "?");
        }
    }
}