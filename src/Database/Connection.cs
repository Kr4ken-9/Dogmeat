using System;

namespace Dogmeat.Database
{
    public class Connection
    {
        private String server;
        private String database;
        private String uid;
        private String password;
        private int port;

        public string Server { get => server; }
        public string Database { get => database; }
        public string UID { get => uid; }
        public string Password { get => password; }
        public int Port { get => port; }

        public Connection(String SERVER, String DATABASE, String UID, String PASSWORD, int PORT)
        {
            server = SERVER;
            database = DATABASE;
            uid = UID;
            password = PASSWORD;
            port = PORT;
        }
    }
}