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

            lock (Connection)
            {
                try
                {
                    Connection.OpenAsync().GetAwaiter().GetResult();

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
                finally { Connection.Close(); }
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