using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Database
{
    public class ExperienceHandler
    {   
        public event EventHandler<ExperienceEventArgs> ExperienceUpdate;

        public void OnExperienceUpdate(UUser User, ushort Experience) => 
            ExperienceUpdate(this, new ExperienceEventArgs(User, Experience));
        
        public async Task IncreaseExperience(ulong ID, ushort Experience)
        {
            MySqlCommand Command = Vars.DBHandler.Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.Parameters.AddWithValue("Experience", Experience);
            Command.CommandText = "UPDATE Users SET Experience = Experience + @Experience WHERE ID = @ID";
            
            await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.NONQUERY);
        }

        public static Byte CalculateExperience() => (Byte) Vars.Random.Next(0, 11);
    }
    
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