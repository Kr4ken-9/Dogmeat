using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Database;
using Microsoft.EntityFrameworkCore;

namespace Dogmeat.Commands
{
    public class DatabaseCommands : ModuleBase
    {
        [Command("tag"), Summary("Retrieves a user-configurable output for given tag")]
        public async Task Tag([Summary("User to retrieve profile for")] String ID, [RemainderAttribute] String Body = "")
        {
            using (DatabaseHandler DbContext = new DatabaseHandler())
            {
                await DbContext.Database.EnsureCreatedAsync();

                Tag Tag = await DbContext.Tags.FirstOrDefaultAsync(tag => tag.ID == ID);

                if (Tag == null)
                {
                    if (String.IsNullOrEmpty(Body) || String.IsNullOrEmpty(ID))
                    {
                        ReplyAsync("ID and Body must not be empty or null");
                        return;
                    }

                    if (ID.Length > 20)
                    {
                        ReplyAsync("ID must be ten characters or less");
                        return;
                    }

                    if (Body.Length > 2000)
                    {
                        ReplyAsync("Body must be two thousand characters or less");
                        return;
                    }

                    await DbContext.Tags.AddAsync(new Tag(ID, Body, Context.User.Id));
                    await DbContext.SaveChangesAsync();

                    ReplyAsync($"Added tag `{ID}` with Body: ```{Body}```");
                    return;
                }

                if (Tag.Owner == Context.User.Id && !String.IsNullOrEmpty(Body))
                {
                    if (MentionUtils.TryParseUser(Body, out ulong userId))
                    {
                        DbContext.Tags.Update(Tag);
                        Tag.Owner = userId;
                        await DbContext.SaveChangesAsync();

                        ReplyAsync($"Owner for tag `{ID}` changed to `{userId}`");
                        return;
                    }

                    if (Body.Length > 2000)
                    {
                        ReplyAsync("Body must be two thousand characters or less");
                        return;
                    }

                    DbContext.Tags.Update(Tag);
                    Tag.Body = Body;
                    await DbContext.SaveChangesAsync();

                    ReplyAsync($"Body for tag `{ID}` changed to ```{Body}```");
                    return;
                }

                ReplyAsync(Tag.Body);
            }
        }

        [Command("insignias"), Summary("Retrieves insignias for given user")]
        public async Task Insignias([Summary("User to retrieve insignias for")] IUser Target = null)
        {
            UUser UTarget;

            using (DatabaseHandler DbContext = new DatabaseHandler())
            {
                await DbContext.Database.EnsureCreatedAsync();

                if (Target == null)
                {
                    Target = Context.User;
                    UTarget = await DbContext.Users.FirstOrDefaultAsync(user => user.ID == Target.Id);

                    if (UTarget == null)
                    {
                        ReplyAsync($"{Context.User.Id} is not in the database.");
                        return;
                    }

                    if (UTarget.Insignias == "None")
                    {
                        ReplyAsync($"{Target.Mention} has no insignias.");
                        return;
                    }

                    ReplyAsync("", embed: await GenerateInsignias(Target, await
                        Utilities.Insignias.GetInsignias(UTarget.Insignias)));

                    return;
                }

                UTarget = await DbContext.Users.FirstOrDefaultAsync(user => user.ID == Target.Id);

                if (UTarget == null)
                {
                    ReplyAsync($"{Context.User.Id} is not in the database.");
                    return;
                }

                if (UTarget.Insignias == "None")
                {
                    ReplyAsync($"{Target.Mention} has no insignias.");
                    return;
                }

                ReplyAsync("", embed: await GenerateInsignias(Target,
                    await Utilities.Insignias.GetInsignias(UTarget.Insignias)));
            }
        }

        private async Task<Embed> GenerateInsignias(IUser Target, IEnumerable<Insignia> Insignias)
        {
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>();

            foreach (Insignia I in Insignias)
                Fields.Add(await Utilities.Commands.CreateEmbedFieldAsync(I.ID, I.Name));


            return await Utilities.Commands.CreateEmbedAsync($"Insignias for {Target.Username}",
                Colors.SexyPurple, Insignias.First().URL, "", Fields);
        }
    }
}