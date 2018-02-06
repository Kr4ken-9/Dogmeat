using System;
using System.ComponentModel.DataAnnotations;

namespace Dogmeat.Database
{
    public class Insignia
    {
        [MaxLength(20)]
        public string ID { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string URL { get; set; }
        
        // Entity Framework requires parameterless constructor
        public Insignia() {}

        public Insignia(String Id, String name, String url)
        {
            ID = Id;
            Name = name;
            URL = url;
        }
    }
}