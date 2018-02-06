using System;
using System.ComponentModel.DataAnnotations;

namespace Dogmeat.Database
{
    public class UUser
    {
        public ulong ID { get; set; }
        
        public ulong Experience { get; set; }
        
        public ushort Level { get; set; }
        
        [MaxLength(150)]
        public string Description { get; set; }
        
        [MaxLength(300)]
        public string Insignias { get; set; }
        
        public DateTime LastChat { get; set; }
        
        // Entity Framework requires parameterless constructor
        public UUser() {}

        public UUser(ulong id, ulong experience, ushort level, String description, String insignias, DateTime lastChat)
        {
            ID = id;
            Experience = experience;
            Level = level;
            Description = description;
            Insignias = insignias;
            LastChat = lastChat;
        }
    }
}