using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Database
{
    public class UserInfoHandler
    {
        public String ConnectionString;

        public UserInfoHandler(String connectionString)
        {
            ConnectionString = connectionString;
            ExpHandler = new ExperienceHandler(connectionString);
        }

        public ExperienceHandler ExpHandler;

        public async Task AddUser(UUser User) => AddUser(User.ID, User.Experience, User.Description, User.Insignias);

        public async Task AddUser(ulong ID, ushort Experience = 0, String Description = "None", String Insignias = "None")
        {
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.Parameters.AddWithValue("Experience", Experience);
                    Command.Parameters.AddWithValue("Description", Description);
                    Command.Parameters.AddWithValue("Insignias", Insignias);
                    Command.CommandText = "INSERT INTO Users VALUES(@ID, @Experience, 0, @Description, @Insignias, now())";

                    await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.NONQUERY);
                }
            }
        }

        public async Task<UUser> GetUser(ulong ID)
        {
            UUser User = new UUser(ID, 0, 0, "", "", DateTime.MinValue);

            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.CommandText = "SELECT * FROM Users WHERE ID = @ID";

                    using (MySqlDataReader Reader = Command.ExecuteReader())
                    {
                        while (Reader.ReadAsync().GetAwaiter().GetResult())
                        {
                            User.Experience = (ushort)Reader.GetInt16(1);
                            User.Level = (ushort)Reader.GetInt16(2);
                            User.Description = Reader.GetString(3);
                            User.Insignias = Reader.GetString(4);
                            User.LastChat = Reader.GetDateTime(5);
                        }
                    }
                }
            }

            return User;
        }

        public async Task<bool> CheckUser(ulong ID)
        {
            bool Exists = false;
            using (MySqlConnection c = new MySqlConnection(ConnectionString))
            {
                await c.OpenAsync();
                using (MySqlCommand Command = c.CreateCommand())
                {
                    Command.Parameters.AddWithValue("ID", ID);
                    Command.CommandText = "SELECT EXISTS(SELECT 1 FROM Users WHERE ID = @ID LIMIT 1);";

                    object Result =
                        await Utilities.MySql.ExecuteCommand(Command, Utilities.MySql.CommandExecuteType.SCALAR);

                    if (Result == null) return Exists;

                    Int32.TryParse(Result.ToString(), out int exists);

                    Exists = exists != 0;

                    return Exists;
                }
            }
        }
    }
}