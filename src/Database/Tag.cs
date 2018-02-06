using System.ComponentModel.DataAnnotations;

namespace Dogmeat.Database
{
    public class Tag
    {
        [MaxLength(50)]
        public string ID { get; set; }
        
        [MaxLength(2000)]
        public string Body { get; set; }

        public ulong Owner { get; set; }

        // Entity Framework requires parameterless constructor
        public Tag() {}
        
        public Tag(string id, string body, ulong owner)
        {
            ID = id;
            Body = body;
            Owner = owner;
        }
    }
}