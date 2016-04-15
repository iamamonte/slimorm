using System.Data;
using System.Data.SqlClient;

namespace SlimOrm
{
    public class DbConnectionService : IDbConnectionService
    {
        private readonly string _connectionString;
        public DbConnectionService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            return conn;
        }
    }
}
