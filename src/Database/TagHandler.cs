using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Database
{
    public class TagHandler
    {
        public MySqlConnection Connection;

        public TagHandler(MySqlConnection connection) => Connection = connection;

        public async Task AddTag(String ID, String Body, ulong Owner)
        {
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("ID", ID);
            Command.Parameters.AddWithValue("Body", Body);
            Command.Parameters.AddWithValue("Owner", Owner);
            Command.CommandText = "INSERT INTO Tags VALUES(@ID, @Body, @Owner)";

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
        
        public async Task<Tag> GetTag(String ID)
        {
            Tag Output = new Tag(ID, "", uint.MinValue);

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
                        {
                            Output.Body = Reader.GetString(1);
                            Output.Owner = (ulong)Reader.GetValue(2);
                        }
                }
                catch (Exception e) { Console.WriteLine(e); }
                finally { Connection.Close(); }
            }

            return Output;
        }
        
        #region UpdateTag

        public async Task UpdateTag(String ID, ulong Owner) => UpdateTag(ID, "", Owner);

        public async Task UpdateTag(String ID, String Body) => UpdateTag(ID, Body, ulong.MinValue);

        public async Task UpdateTag(String ID, String Body, ulong Owner)
        {
            lock (Vars.DBHandler.Connection)
            {
                try
                {
                    MySqlCommand Command = Connection.CreateCommand();
                    Command.Parameters.AddWithValue("ID", ID);
                    
                    if (!String.IsNullOrEmpty(Body) && Owner != ulong.MinValue)
                    {
                        Command.Parameters.AddWithValue("Body", Body);
                        Command.Parameters.AddWithValue("Owner", Owner);
                        Command.CommandText = "UPDATE Tags SET Body = @Body, Owner = @Owner WHERE ID = @ID";
                    }
                    else if (!String.IsNullOrEmpty(Body))
                    {
                        Command.Parameters.AddWithValue("Body", Body);
                        Command.CommandText = "UPDATE Tags SET Body = @Body WHERE ID = @ID";
                    }
                    else
                    {
                        Command.Parameters.AddWithValue("Owner", Owner);
                        Command.CommandText = "UPDATE Tags SET Owner = @Owner WHERE ID = @ID";
                    }

                    Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.NONQUERY);
                }
                catch (Exception e) { Console.WriteLine(e); }
                finally { Connection.Close(); }
            }
        }
        
        #endregion
    }
}