using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.Database;

namespace Dogmeat.Commands
{
    public class UserInfoCommands : ModuleBase
    {
        [Command("profile"), Summary("Retrieves a user's profile from mysql database")]
        public async Task Profile([Summary("User to retrieve profile for")] IGuildUser Target)
        {
            if (!await Vars.DBHandler.UUIHandler.CheckUser(Target.Id))
            {
                ReplyAsync($"{Target.Mention} is not in database.");
                return;
            }

            UUser User = await Vars.DBHandler.UUIHandler.GetUser(Target.Id);
            
            List<Action<EmbedFieldBuilder>> Fields = new List<Action<EmbedFieldBuilder>>
            {
                await Utilities.Commands.CreateEmbedFieldAsync("Experience", User.Experience),
                await Utilities.Commands.CreateEmbedFieldAsync("Level", User.Level),
                await Utilities.Commands.CreateEmbedFieldAsync("Global Rank", User.Global),
                await Utilities.Commands.CreateEmbedFieldAsync("Description", User.Description)
            };

            Embed Embed = await Utilities.Commands.CreateEmbedAsync($"User info for {Target.Username}",
                Colors.SexyPurple, Target.GetAvatarUrl(), "", Fields.ToArray());

            ReplyAsync("", embed: Embed);
        }
        
        [Command("tag"), Summary("Retrieves a user-configurable output for given tag")]
        public async Task Tag([Summary("User to retrieve profile for")] String ID, String Body = "")
        {
            if (!await Vars.DBHandler.TagHandler.CheckTag(ID))
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

                if (Body.Length > 50)
                {
                    ReplyAsync("Body must be fifty characters or less");
                    return;
                }
                
                Vars.DBHandler.TagHandler.AddTag(ID, Body);
                ReplyAsync($"Added tag {ID} with Body: {Body}");
                return;
            }

            String RetrievedBody = await Vars.DBHandler.TagHandler.GetTag(ID);

            ReplyAsync(RetrievedBody);
        }
    }
}