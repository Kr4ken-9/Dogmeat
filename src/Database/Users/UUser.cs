using System;

namespace Dogmeat.Database
{
    public class UUser
    {
        private ulong id;
        private String insignias;
        private String description;
        private DateTime lastChat;
        private ushort experience;
        private ushort level;
        

        public ulong ID { get => id; }
        public ushort Experience { get => experience; }
        public ushort Level { get => level; }
        public string Description { get => description; }
        public string Insignias { get => insignias; }
        public DateTime LastChat { get => lastChat; }

        public UUser(ulong Id, ushort Experience, ushort Level, String Description, String Insignias, DateTime LastChat)
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