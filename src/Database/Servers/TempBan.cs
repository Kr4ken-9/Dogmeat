using System;

namespace Dogmeat.Database.Servers
{
    public class TempBan
    {
        public ulong ID { get; set; }
        public ulong ServerID { get; set; }
        public DateTime UnbanTime { get; set; }
        
        public TempBan() {}

        public TempBan(ulong id, ulong serverid, DateTime unbantime)
        {
            ID = id;
            ServerID = serverid;
            UnbanTime = unbantime;
        }
    }
}