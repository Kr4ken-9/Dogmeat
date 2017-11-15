using System;

namespace Dogmeat.Database
{
    public class ExperienceEventArgs : EventArgs
    {
        public UUser User;
        public ushort Amount;

        public ExperienceEventArgs(UUser user, ushort amount)
        {
            User = user;
            Amount = amount;
        }
    }
}