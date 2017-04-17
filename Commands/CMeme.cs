using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace DogMeat.Commands
{
    public class CMeme : ModuleBase
    {
        [Command("meme"), Summary("Only the dankest")]
        public async Task MemeAsync()
        {
            String[] Memes = await Utilities.DogmeatMemesAsync();
            Random r = new Random();
            int index = r.Next(0, Memes.Length + 1);
            ReplyAsync(Memes[index]);
        }
    }
}
