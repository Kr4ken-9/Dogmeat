using System;

namespace Dogmeat.Database
{
    public class Tag
    {
        private String body;
        private String id;
        private ulong owner;

        public string ID { get => id; set => id = value; }
        public string Body { get => body; set => body = value; }
        public ulong Owner { get => owner; set => owner = value; }

        public Tag(string id, string body, ulong owner)
        {
            ID = id;
            Body = body;
            Owner = owner;
        }
    }
}