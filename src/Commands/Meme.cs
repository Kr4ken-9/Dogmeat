using System.Threading.Tasks;
using Discord.Commands;

namespace Dogmeat.Commands
{
    public class CMeme : ModuleBase
    {
        [Command("meme"), Summary("Only the dankest")]
        public async Task MemeAsync()
        {
            int index = Vars.Random.Next(0, Vars.Memes.Length + 1);
            ReplyAsync(Vars.Memes[index]);
        }
    }
}
