using System;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Utilities
{
    public class MySql
    {
        public static async Task<object> ExecuteCommand(MySqlCommand Command, CommandExecuteType ExeType)
        {
            object Result = null;

            lock (Vars.DBHandler.Connection)
            {
                try
                {
                    Vars.DBHandler.Connection.OpenAsync().GetAwaiter().GetResult();

                    switch (ExeType)
                    {
                        case CommandExecuteType.NONQUERY:
                            Result = Command.ExecuteNonQueryAsync().GetAwaiter().GetResult();
                            break;
                        case CommandExecuteType.SCALAR:
                            Result = Command.ExecuteScalarAsync().GetAwaiter().GetResult();
                            break;
                    }
                }
                catch (Exception Ex) { Console.WriteLine(Ex); }
                finally { Vars.DBHandler.Connection.Close(); }
            }

            return Result;
        }

        public enum CommandExecuteType
        {
            NONQUERY,
            SCALAR
        }
    }
}