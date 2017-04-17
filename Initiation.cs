using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discord.WebSocket;
using Discord;

namespace DogMeat
{
    public class Initiation
    {
        private static string path = AssemblyDirectory() + "\\ServerList.json";

        public static List<Server> Servers;

        private static void UpdateServer()
        {

        }

        private static async Task Initiate(SocketMessage Context, Server Server)
        {
            await ((IGuildUser)Context.Author).AddRoleAsync(Utilities.GetRole(((SocketGuildChannel)Context.Channel).Guild, Server.InitiationRole));
            await Context.DeleteAsync();
        }

        public static async Task HandleInitiationAsync(SocketMessage Context)
        {
            for(int i = 0; i < Servers.Count; i++)
            {
                if(Servers[i].InitiationChannel == Context.Channel.Id)
                {
                    if (Servers[i].InitiationPhrase.ToUpperInvariant() == Context.Content.ToUpperInvariant())
                    {
                        await Initiate(Context, Servers[i]);
                        return;
                    }
                    await Utilities.WrongChannelAsync(Context);
                }
            }
        }

        public static string AssemblyDirectory()
        {
            string codeBase = Assembly.GetEntryAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static void CheckServerList()
        {
            if (!File.Exists(path))
                File.Create(path);
        }

        public static List<Server> GetServerList()
        {
            if (File.ReadAllText(path) != null)
                return (List<Server>)JsonConvert.DeserializeObject(File.ReadAllText(path));
            return null;
        }

        public static void SaveServerList()
        {
            if (Servers != null)
                File.WriteAllText(path, JsonConvert.SerializeObject(Servers));
        }

        public static void AddServer(ulong ID, ulong Channel, String Phrase, ulong Role)
        {
            Server Input = new Server(ID, Channel, Phrase, Role);
            if (Servers.Contains(Input))
                return;
            Servers.Add(Input);
        }
    }
}