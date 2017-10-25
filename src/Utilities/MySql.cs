using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Utilities
{
    public class MySql
    {
        public static async Task<object> ExecuteCommand(MySqlConnection Connection, MySqlCommand Command, CommandExecuteType ExeType)
        {
            object Result = null;
            
            try
            {
                await Connection.OpenAsync();
                
                switch (ExeType)
                {
                    case CommandExecuteType.NONQUERY:
                        Result = await Command.ExecuteNonQueryAsync();
                        break;
                    case CommandExecuteType.SCALAR:
                        Result = await Command.ExecuteScalarAsync();
                        break;
                }
            }
            catch(Exception ex) { Console.WriteLine(ex); }
            finally { Connection.Close(); }

            return Result;
        }

        public enum CommandExecuteType
        {
            NONQUERY,
            SCALAR
        }
    }
}