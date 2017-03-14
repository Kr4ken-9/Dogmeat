using System;
using System.Collections.Generic;
using System.Text;

namespace DogMeat
{
    public class Server
    {
        public ulong InitiationChannel;

        public ulong InitiationRole;

        public string InitiationPhrase;

        public ulong ID;

        private Server() { }

        public Server(ulong id, ulong initiationchannel, string initiationphrase, ulong initiationrole)
        {
            InitiationChannel = initiationchannel;
            InitiationPhrase = initiationphrase;
            ID = id;
            InitiationRole = initiationrole;
        }
    }
}
