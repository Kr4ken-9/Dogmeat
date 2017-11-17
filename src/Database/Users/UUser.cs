using System;

namespace Dogmeat.Database
{
    public class UUser
    {
        public ulong ID;
        
        public ushort Experience, Level;
        
        public uint Global;

        public String Description, Insignias;

        public DateTime LastChat;

        public UUser(ulong id, ushort experience, ushort level, uint global, String description, String insignias, DateTime lastChat)
        {
            ID = id;
            Experience = experience;
            Level = level;
            Global = global;
            Description = description;
            Insignias = insignias;
            LastChat = lastChat;
        }
    }
}