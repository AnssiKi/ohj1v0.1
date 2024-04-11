using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace ohj1v0._1
{
    public class DatabaseConnector
    {
        private readonly string server = "localhost";
        private readonly string port = "3307";
        private readonly string uid = "root";
        private readonly string pwd = "Ruutti";
        private readonly string database = "vn";
        public DatabaseConnector()
        {
        }
        public MySqlConnection _getConnection()
        {
            string connectionString =
           $"Server={server};Port={port};uid={uid};password={pwd};database={database}";

            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }
    }
}
