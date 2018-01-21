using System;

namespace Dogmeat.Database
{
    public class Tag
    {
        public String ID, Body;

        public ulong Owner;

        public Tag(string id, string body, ulong owner)
        {
            ID = id;
            Body = body;
            Owner = owner;
        }
    }
}