using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Dogmeat.Database
{
    public class UserInfoHandler
    {
        private String connectionString;
        private ExperienceHandler expHandler;

        public string ConnectionString { get => connectionString; }
        public ExperienceHandler ExpHandler { get => expHandler; }

        public UserInfoHandler(String ConnectionString)
        {
            connectionString = ConnectionString;
            expHandler = new ExperienceHandler(ConnectionString);
        }

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

                    await Command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<UUser> GetUser(ulong ID)
        {
            UUser User = null;

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
                            ushort exp = (ushort)Reader.GetInt16(1);
                            ushort level = (ushort)Reader.GetInt16(2);
                            string description = Reader.GetString(3);
                            string insignias = Reader.GetString(4);
                            DateTime lastChat = Reader.GetDateTime(5);
                            User = new UUser(ID, exp, level, description, insignias, DateTime.MinValue);
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

                    object Result = await Command.ExecuteScalarAsync();

                    if (Result == null) return Exists;

                    Int32.TryParse(Result.ToString(), out int exists);

                    Exists = exists != 0;

                    return Exists;
                }
            }
        }
    }
}