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
                await c.OpenAsync();
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

            return Result;
        }

        public enum CommandExecuteType
        {
            NONQUERY,
            SCALAR
        }
    }
}