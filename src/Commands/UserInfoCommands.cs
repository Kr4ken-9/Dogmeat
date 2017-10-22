using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Dogmeat.UUI;

namespace Dogmeat.Commands
{
    public class UserInfoCommands : ModuleBase
    {
        [Command("profile"), Summary("Retrieves a user's profile from mysql database")]
        public async Task Profile([Summary("User to retrieve profile for")] IGuildUser Target)
        {
            if (!await Vars.UUIHandler.CheckUser(Target.Id))
            {
                ReplyAsync($"{Target.Mention} is not in database.");
                return;
            }

            UUser User = await Vars.UUIHandler.GetUser(Target.Id);
            
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
    }
}