using System;
using System.ComponentModel.DataAnnotations;

namespace Dogmeat.Database.Servers
{
    public class Reminder
    {
        public ulong ID { get; set; }
        public ulong ServerID { get; set; }
        public ulong ChannelID { get; set; }
        public DateTime RemindDate { get; set; }
        
        [MaxLength(2000)]
        public string Content { get; set; }
        
        public Reminder() {}

        public Reminder(ulong id, ulong serverid, ulong channelid, DateTime reminddate, string content)
        {
            ID = id;
            ServerID = serverid;
            ChannelID = channelid;
            RemindDate = reminddate;
            Content = content;
        }
    }
}