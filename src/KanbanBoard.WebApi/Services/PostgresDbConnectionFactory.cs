using System.Data;
using Npgsql;

namespace KanbanBoard.WebApi.Services
{
    public class PostgresDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public PostgresDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
