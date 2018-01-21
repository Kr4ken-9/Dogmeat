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

            using (MySqlConnection c = new MySqlConnection(Vars.DBHandler.ConnectionString))
            {
                try
                {
                    c.OpenAsync().GetAwaiter().GetResult();
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
                finally { c.Close(); }
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