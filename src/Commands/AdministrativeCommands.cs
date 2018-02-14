using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Database;
using Dogmeat.Database.Servers;
using Dogmeat.Utilities;

namespace Dogmeat.Commands
{
    public class Administrative : ModuleBase
    {
        [Command("ban"), Summary("Bans a user")]
        public async Task Ban([Summary("User of person to ban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;
            
            ReplyAsync($"{User.Mention} is no more.");

            if (Reason == null)
                User.Guild.AddBanAsync(User, 7);
            else
                User.Guild.AddBanAsync(User, 7, Reason);
        }
        
        [Command("kick"), Summary("Kicks specified user")]
        public async Task Kick([Summary("User to kick")] IGuildUser User, [Summary("Reason for kick")] String Reason = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;
                
            ReplyAsync($"{User.Mention} is no more.");
            User.KickAsync(Reason);
        }
        
        [Command("mute"), Summary("Mutes a user")]
        public async Task Mute([Summary("User to mute")] IGuildUser User)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, User, Context.Channel)) return;

            IRole Muted = null;

            if (Misc.GetMutedRole(Context.Guild) == null)
            {
                Muted = await Utilities.Commands.CreateMutedRoleAsync(Context.Guild);
                ReplyAsync("Muted role created.");
            }

            if (Muted == null)
                Muted = Misc.GetMutedRole(Context.Guild);
            
            if (!User.RoleIds.Contains(Muted.Id))
            {
                User.AddRoleAsync(Muted);
                ReplyAsync($"{User.Mention} has been muted.");
            }
            else
            {
                User.RemoveRoleAsync(Muted);
                ReplyAsync($"{User.Mention} has been unmuted.");
            }
        }
        
        [Command("purge"), Alias("prune"), Summary("Purges messages on executed channel.")]
        public async Task Purge([Summary("Number of messages to purge")] int count, [Summary("User to prune")] IGuildUser User = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;

            if (User == null)
            {
                Context.Channel.DeleteMessagesAsync(await Context.Channel.GetMessagesAsync(count).Flatten());

                IEnumerable<IMessage> DeleteMe =
                    new List<IMessage> {await Context.Channel.SendMessageAsync($"Purged {count} messages.")};

                Thread.Sleep(5000);

                Context.Channel.DeleteMessagesAsync(DeleteMe);
            }
            else
            {
                IEnumerable<IMessage> Messages = await Context.Channel.GetMessagesAsync(count).Flatten();
                IEnumerable<IMessage> UserMessages = Messages.Where(Message => Message.Author == User);

                Context.Channel.DeleteMessagesAsync(UserMessages);

                IEnumerable<IMessage> DeleteMe = new List<IMessage>
                {
                    await Context.Channel.SendMessageAsync($"Purged {count} messages from {User.Mention}")
                };

                Thread.Sleep(5000);
                
                Context.Channel.DeleteMessagesAsync(DeleteMe);
            }
        }
        
        [Command("softban"), Summary("Bans a user without removing his messages")]
        public async Task SoftBan([Summary("User to softban")] IGuildUser User, [Summary("Reason for ban")] String Reason = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;
            
            ReplyAsync($"{User.Mention} is no more.");

            if (Reason == null)
                User.Guild.AddBanAsync(User);
            else
                User.Guild.AddBanAsync(User, 0, Reason);
        }

        [Command("tempban"), Summary("Bans a user for a specified amount of time")]
        public async Task TempBan([Summary("User to tempban")] IGuildUser User,
            [Summary("Amount of time to ban")] String Length, [Summary("Reason for ban")] String Reason = null)
        {
            if (!await Utilities.Commands.CommandMasterAsync(Context.Guild, Context.User, Context.Channel)) return;

            DateTime UnbanDate = Utilities.Commands.ParseDateFromString(Length.ToUpperInvariant());

            ReplyAsync($"{User.Mention} banned until {UnbanDate.ToLongDateString()}");

            if (Reason == null)
                User.Guild.AddBanAsync(User, 7);
            else
                User.Guild.AddBanAsync(User, 7, Reason);

            using (DatabaseHandler DbContext = new DatabaseHandler())
            {
                await DbContext.Database.EnsureCreatedAsync();
                
                TempBan TBan = new TempBan(User.Id, User.Guild.Id, UnbanDate);
                await DbContext.TempBans.AddAsync(TBan);
                await DbContext.SaveChangesAsync();
            }
        }
    }
}