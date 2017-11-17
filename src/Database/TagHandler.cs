using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Database
{
    public class TagHandler
    {
        public MySqlConnection Connection;

        public TagHandler(MySqlConnection connection) => Connection = connection;

        public async Task AddTag(String ID, String Body)
        {
            if(ID.Length > 10)
                throw new Exception("ID limit is ten characters");
            
            if(Body.Length > 50)
                throw new Exception("Description limit is fifty characters");

            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.Parameters.AddWithValue("Body", Body);
            Command.CommandText = "INSERT INTO Tags VALUES(@ID, @Body)";

            await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.NONQUERY);
        }
        
        public async Task<bool> CheckTag(String ID)
        {
            bool Exists = false;
            
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.CommandText = "SELECT EXISTS(SELECT 1 FROM Tags WHERE ID = @ID LIMIT 1);";

            object Result =
                await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.SCALAR);

            if (Result == null) return Exists;
            
            Int32.TryParse(Result.ToString(), out int exists);

            Exists = exists != 0;

            return Exists;
        }
        
        public async Task<String> GetTag(String ID)
        {
            String Body = "";

            lock (Vars.DBHandler.Connection)
            {
                try
                {
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.CommandText = "SELECT * FROM Tags WHERE ID = @ID";

                    Connection.OpenAsync().GetAwaiter().GetResult();

                    using (MySqlDataReader Reader = Command.ExecuteReader())
                        while (Reader.ReadAsync().GetAwaiter().GetResult())
                            Body = Reader.GetString(1);
                }
                catch (Exception e) { Console.WriteLine(e); }
                finally { Connection.Close(); }
            }

            return Body;
        }
    }
}