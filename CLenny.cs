using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DogMeat
{
    public class CLenny : ModuleBase
    {
        [Command("lenny"), Summary("Prints a lenny face.")]
        public async Task LennyAsync()
        {
            await ReplyAsync("( ͡° ͜ʖ ͡°)");
        }
    }
}
