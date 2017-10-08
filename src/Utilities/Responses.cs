using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dogmeat.Utilities
{
    public class Responses
    {
        public static async Task<String[]> DogmeatResponsesAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/replies.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatMemesAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/memes.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String[]> DogmeatAnswersAsync()
        {
            string Content = "";
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage Response = await Client.GetAsync("http://198.245.61.226/kr4ken/dogmeat/answers.txt");
                Content = await Response.Content.ReadAsStringAsync();
            }
            
            return Content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public static async Task<String> ResponsePickerAsync(String Content)
        {
            if (Content.Contains("MASTER") ||
                Content.Contains("CREATOR") ||
                Content.Contains("DEVELOPER"))
                return "*I am reluctantly groomed by that faggot Kr4ken*";

            if (Content.Contains("?"))
            {
                String[] Answers = Vars.Answers;
                return Answers[Vars.Random.Next(0, Answers.Length)];
            }

            foreach (var Pair in Vars.TailoredResponses)
            {
                if (Content.Contains(Pair.Key))
                {
                    return Pair.Value.Length == 1
                        ? Vars.RawResponses[Pair.Value.First()]
                        : Vars.RawResponses[Vars.Random.Next(Pair.Value.First(), Pair.Value.Last() + 1)];
                }
            }
            
            return Vars.RawResponses[Vars.Random.Next(0, Vars.RawResponses.Length)];
        }
    }
}