using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DogMeat
{
    public class CInfo : ModuleBase
    {
        [Command("info"), Summary("Prints info regarding Dogmeat.")]
        public async Task Info()
        {
            await ReplyAsync("```" +
                "Dogmeat was created 1/22/17.\r\n" +
                "Dogmeat is using version " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version +
                ".\r\nDogmeat is currently in a closed alpha.\r\n" + 
                "Dogmeat has offended ~420 people with his responses.```");
        }
    }
}
