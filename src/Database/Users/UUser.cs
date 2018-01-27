using System;
using System.ComponentModel.DataAnnotations;

namespace Dogmeat.Database
{
    public class UUser
    {
        private ulong id;
        private String insignias;
        private String description;
        private DateTime lastChat;
        private ulong experience;
        private ushort level;
        
        [Required]
        public ulong ID { get => id; set => id = value; }
        
        [Required]
        public ulong Experience { get => experience; set => experience = value; }
        
        [Required]
        public ushort Level { get => level; set => level = value; }
        
        [Required]
        public string Description { get => description; set => description = value; }
        
        [Required]
        public string Insignias { get => insignias; set => insignias = value; }
        
        [Required]
        public DateTime LastChat { get => lastChat; set => lastChat = value; }
        
        // Entity Framework requires parameterless constructor
        public UUser() {}

        public UUser(ulong Id, ulong Experience, ushort Level, String Description, String Insignias, DateTime LastChat)
        {
            ID = Id;
            this.Experience = Experience;
            this.Level = Level;
            this.Description = Description;
            this.Insignias = Insignias;
            this.LastChat = LastChat;
        }
    }
}