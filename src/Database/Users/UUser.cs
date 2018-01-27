using System;

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
        
        public ulong ID { get => id; set => id = value; }
        public ulong Experience { get => experience; set => experience = value; }
        public ushort Level { get => level; set => level = value; }
        public string Description { get => description; set => description = value; }
        public string Insignias { get => insignias; set => insignias = value; }
        public DateTime LastChat { get => lastChat; set => lastChat = value; }

        public UUser(ulong Id, ulong Experience, ushort Level, String Description, String Insignias, DateTime LastChat)
        {
            id = Id;
            experience = Experience;
            level = Level;
            description = Description;
            insignias = Insignias;
            lastChat = LastChat;
        }
    }
}