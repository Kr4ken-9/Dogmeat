using System;

namespace Dogmeat.Database
{
    public class UUser
    {
        public ulong ID;
        
        public ushort Experience, Level;
        
        public uint Global;

        public String Description, Insignia;

        public DateTime LastChat;

        public UUser(ulong id, ushort experience, ushort level, uint global, String description, String insignia, DateTime lastChat)
        {
            ID = id;
            Experience = experience;
            Level = level;
            Global = global;
            Description = description;
            Insignia = insignia;
            LastChat = lastChat;
        }
    }
}