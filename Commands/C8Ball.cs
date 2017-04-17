using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DogMeat.Commands
{
    public class C8Ball : ModuleBase
    {
        [Command("8ball"), Summary("A trusted source.")]
        public async Task EightBallAsync()
        {
            String[] Answers = await Utilities.DogmeatAnswersAsync();
            await ReplyAsync(Answers[new Random().Next(0, Answers.Length + 1)]);
        }
    }
}