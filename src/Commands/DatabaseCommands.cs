using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Database;

namespace Dogmeat.Commands
{
    public class DatabaseCommands : ModuleBase
    {
        [Command("profile"), Summary("Retrieves a user's profile from mysql database")]
        public async Task Profile([Summary("User to retrieve profile for")] IUser Target = null)
        {
            UUser User;
            
            if (Target == null)
            {
                if (!await Vars.DBHandler.UUIHandler.CheckUser(Context.User.Id))
                {
                    ReplyAsync($"{Context.User.Mention} is not in database.");
                    return;
                }

                User = await Vars.DBHandler.UUIHandler.GetUser(Context.User.Id);
                
                ReplyAsync("", embed: await GenerateProfile(User, Context.User));

                return;
            }
            
            if (!await Vars.DBHandler.UUIHandler.CheckUser(Target.Id))
            {
                ReplyAsync($"{Target.Mention} is not in database.");
                return;
            }

            User = await Vars.DBHandler.UUIHandler.GetUser(Target.Id);

            ReplyAsync("", embed: await GenerateProfile(User, Target));
        }

        private async Task<Embed> GenerateProfile(UUser User, IUser Target)
        {
            Action<EmbedFieldBuilder>[] Fields =
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Experience", User.Experience),
                await Utilities.Commands.CreateEmbedFieldAsync("Level", User.Level),
                //await Utilities.Commands.CreateEmbedFieldAsync("Global Rank", User.Global), 
                await Utilities.Commands.CreateEmbedFieldAsync("Description", User.Description)
            };

            return await Utilities.Commands.CreateEmbedAsync($"User info for {Target.Username}",
                Colors.SexyPurple, Target.GetAvatarUrl(), "", Fields);
        }
        
        [Command("tag"), Summary("Retrieves a user-configurable output for given tag")]
        public async Task Tag([Summary("User to retrieve profile for")] String ID, [RemainderAttribute] String Body = "")
        {
            if (!await Vars.DBHandler.Tags.CheckTag(ID))
            {
                if (String.IsNullOrEmpty(Body) || String.IsNullOrEmpty(ID))
                {
                    ReplyAsync("ID and Body must not be empty or null");
                    return;
                }
                
                if (ID.Length > 10)
                {
                    ReplyAsync("ID must be ten characters or less");
                    return;
                }

                if (Body.Length > 3000)
                {
                    ReplyAsync("Body must be three thousand characters or less");
                    return;
                }
                
                Vars.DBHandler.Tags.AddTag(ID, Body, Context.User.Id);
                ReplyAsync($"Added tag `{ID}` with Body: ```{Body}```");
                return;
            }

            Tag Tag = await Vars.DBHandler.Tags.GetTag(ID);

            if (Tag.Owner == Context.User.Id && !String.IsNullOrEmpty(Body))
            {
                if (MentionUtils.TryParseUser(Body, out ulong userId))
                {
                    Vars.DBHandler.Tags.UpdateTag(ID, userId);
                    
                    ReplyAsync($"Owner for tag `{ID}` changed to `{userId}`");
                    return;
                }
                
                if (Body.Length > 3000)
                {
                    ReplyAsync("Body must be three thousand characters or less");
                    return;
                }

                Vars.DBHandler.Tags.UpdateTag(ID, Body);
                ReplyAsync($"Body for tag `{ID}` changed to ```{Body}```");
            }

            ReplyAsync(Body);
        }

        [Command("insignias"), Summary("Retrieves insignias for given user")]
        public async Task Insignias([Summary("User to retrieve insignias for")] IUser Target = null)
        {
            UUser UTarget;
            
            if (Target == null)
            {
                Target = Context.User;
                
                if (!await Vars.DBHandler.UUIHandler.CheckUser(Target.Id))
                {
                    await ReplyAsync($"{Context.User.Id} is not in the database.");
                    return;
                }

                UTarget = await Vars.DBHandler.UUIHandler.GetUser(Target.Id);

                if (UTarget.Insignias == "None")
                {
                    await ReplyAsync($"{Target.Mention} has no insignias.");
                    return;
                }

                ReplyAsync("", embed: await GenerateInsignias(Target, await
                    Vars.DBHandler.Insignias.GetInsignia(UTarget.Insignias)));
                
                return;
            }
            
            if (!await Vars.DBHandler.UUIHandler.CheckUser(Target.Id))
            {
                await ReplyAsync($"{Context.User.Id} is not in the database.");
                return;
            }

            UTarget = await Vars.DBHandler.UUIHandler.GetUser(Target.Id);
            
            if (UTarget.Insignias == "None")
            {
                await ReplyAsync($"{Target.Mention} has no insignias.");
                return;
            }

            ReplyAsync("", embed: await GenerateInsignias(Target, 
                await Vars.DBHandler.Insignias.GetInsignia(UTarget.Insignias)));
        }
        
        private async Task<Embed> GenerateInsignias(IUser Target, IEnumerable<Insignia> Insignias)
        {
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>();

            foreach (Insignia I in Insignias)
                Fields.Add(await Utilities.Commands.CreateEmbedFieldAsync(I.ID, I.Name));

            return await Utilities.Commands.CreateEmbedAsync($"Insignias for {Target.Username}",
                Colors.SexyPurple, Insignias.First().URL, "", Fields.ToArray());
        }
    }
}