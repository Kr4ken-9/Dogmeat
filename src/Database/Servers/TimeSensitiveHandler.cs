using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Dogmeat.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using Discord;

namespace Dogmeat.Database.Servers
{
    public class TimeSensitiveHandler
    {
        public static async Task RunConstantChecks()
        {
            while (Vars.KeepAlive)
            {
                using (DatabaseHandler Context = new DatabaseHandler())
                {
                    await Context.Database.EnsureCreatedAsync();

                    var TempBans = Context.TempBans.Where(t => t.UnbanTime <= Vars.Now());
                    if (await TempBans.AnyAsync())
                    {
                        foreach (TempBan Ban in TempBans)
                        {
                            SocketGuild Guild = Vars.Client.GetGuild(Ban.ServerID);
                            SocketUser User = Vars.Client.GetUser(Ban.ID);

                            if (User == null || Guild == null)
                                continue;

                            IDMChannel dmChannel = await User.GetOrCreateDMChannelAsync();

                            try
                            {
                                await Guild.RemoveBanAsync(Ban.ID);
                                dmChannel.SendMessageAsync($"You have been unbanned from {Guild.Name}.");
                            }
                            catch (Discord.Net.HttpException e)
                            {
                                Logger.Log($"An HttpException was thrown while attempting to unban '{User.Username}' from '{Guild.Name}':\n" +
                                    $"Response Code: {e.HttpCode}, Discord Code: {e.DiscordCode}");

                                switch (e.HttpCode)
                                {
                                    case HttpStatusCode.Forbidden:
                                        dmChannel.SendMessageAsync($"You were supposed to be unbanned from {Guild.Name}," +
                                            $" but I lacked the permissions to unban you.");
                                        break;

                                    case HttpStatusCode.NotFound:
                                        dmChannel.SendMessageAsync($"You were supposed to be unbanned from {Guild.Name}," +
                                            $" but it seems that you were already unbanned.");
                                        break;

                                    case default:
                                        dmChannel.SendMessageAsync($"You were supposed to be unbanned from {Guild.Name}," +
                                            $" but something went wrong and I don't know what.");
                                        break;
                                }
                            }

                            Context.TempBans.Remove(Ban); //TODO: Why does this hang thread?!?!

                            Logger.Log($"Unbanned {User.Username}");
                        }
                    }

                    var Reminders = Context.Reminders.Where(r => r.RemindDate <= Vars.Now());
                    if (await Reminders.AnyAsync())
                    {
                        foreach (Reminder Reminder in Reminders)
                        {
                            SocketUser User = Vars.Client.GetUser(Reminder.ID);
                            (await User.GetOrCreateDMChannelAsync()).SendMessageAsync($"Reminder: {Reminder.Content}");

                            SocketGuild Guild = Vars.Client.GetGuild(Reminder.ServerID);
                            SocketTextChannel Channel = Guild.GetTextChannel(Reminder.ChannelID);
                            Channel.SendMessageAsync($"Reminder for {User.Mention}: {Reminder.Content}");

                            Context.Reminders.Remove(Reminder);
                        }
                    }

                    await Context.SaveChangesAsync();

                }
                Logger.Log("Time Sensitives Checked");
                Thread.Sleep(60000);
            }
            Task.Delay(-1);
        }
    }
}