using System;

namespace Dogmeat.UUI
{
    public class UUser
    {
        public ulong ID;
        
        public ushort Experience, Level;
        
        public uint Global;

        public String Description;

        public UUser(ulong id, ushort experience, ushort level, uint global, string description)
        {
            ID = id;
            Experience = experience;
            Level = level;
            Global = global;
            Description = description;
        }
    }
}