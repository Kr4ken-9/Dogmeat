using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DogMeat.Commands
{
    public class CMeme : ModuleBase
    {
        [Command("meme"), Summary("Only the dankest")]
        public async Task MemeAsync()
        {
            String[] Memes = Vars.Memes;
            Random r = new Random();
            int index = r.Next(0, Memes.Length + 1);
            ReplyAsync(Memes[index]);
        }
    }
}
