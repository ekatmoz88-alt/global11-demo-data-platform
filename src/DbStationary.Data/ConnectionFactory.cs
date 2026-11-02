using System.Data.SQLite;

namespace DbStationary.Data
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public class ConnectionFactory
    {
        private static ConnectionFactory _instance;

        internal string ConnectionString = "Data Source=" +
                                            System.IO.Path.GetDirectoryName(
                                                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) +
                                            "\\MainDB.db3;Version=3;";

        private readonly SQLiteConnection _connection;

        private ConnectionFactory()
        {
            var connStr = ConnectionString.Replace(@"file:\", "");
            _connection = new SQLiteConnection(connStr);
            _connection.Open();
        }

        public static SQLiteConnection GetConnection()
        {
            if (_instance == null)
            {
                _instance = new ConnectionFactory();
            }
            return _instance._connection;
        }
    }
}
