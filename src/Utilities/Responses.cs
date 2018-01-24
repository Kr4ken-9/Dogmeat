using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dogmeat.Utilities
{
    public static class Responses
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

            foreach (var Pair in TailoredResponses)
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

        public static readonly Dictionary<String, int[]> TailoredResponses = new Dictionary<String, int[]>
        {
            #region Mexican
            { "JUAN", new[] { 19, 20 } },
            { "MEXICO", new[] { 19, 20 } },
            { "EDUARDO", new[] { 19, 20 } },
            { "TRUMP", new[] { 19, 20 } },
            { "DONALD", new[] { 19, 20 } },
            { "PRESIDENT", new[] { 19, 20 } },
            #endregion
            #region Jew
            { "JEW", new[] { 16, 17, 18 } },
            #endregion
            #region AA
            { "BLACK", new[] { 15 } },
            { "NIGG", new[] { 15 } },
            #endregion
            #region Hillary
            { "HILLARY", new[] { 19 } },
            { "CLINTON", new[] { 19 } },
            { "MEME QUEEN", new [] { 19 } },
            #endregion
            #region Insult
            { "FUCK", new[] { 4 } },
            { "CUNT", new[] { 4 } },
            { "ASS", new[] { 4 } },
            { "DOUCHE", new[] { 4 } },
            { "KYS", new[] { 4 } },
            { "DIE", new[] { 4 } },
            { "ROAST", new[] { 4 } },
            { "COCK", new[] { 4 } },
            #endregion
            #region LGBT
            { "GAY", new[] { 5 } },
            { "LESBIAN", new[] { 5 } },
            { "TRANS", new[] { 5 } },
            { "SEXUAL", new[] { 5 } },
            #endregion
            #region Supremacy
            { "RACIS", new[] { 6 } },
            { "HITLER", new[] { 6 } },
            { "RACE", new[] { 6 } },
            { "ARYA", new[] { 6 } },
            #endregion
            #region Female
            { "WOMEN", new[] { 7 } },
            { "GIRL", new[] { 7 } },
            { "WOMAN", new[] { 7 } },
            { "GRILL", new[] { 7 } },
            { "VAGINA", new[] { 7 } },
            { "PUSSY", new[] { 7 } },
            #endregion
        };
    }
}