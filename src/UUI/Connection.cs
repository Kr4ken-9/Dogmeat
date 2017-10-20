using System;

namespace Dogmeat.UUI
{
    public class Connection
    {
        public String Server, Database, UID, Password;

        public int Port;

        public Connection(String SERVER, String DATABASE, String uid, String PASSWORD, int PORT)
        {
            Server = SERVER;
            Database = DATABASE;
            UID = uid;
            Password = PASSWORD;
            Port = PORT;
        }
    }
}