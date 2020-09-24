using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;

namespace KanbanBoard.IntegrationTests.WebApi
{
    public class TestFixture : IDisposable
    {
        private const string ConnectionString =
            "Host=localhost;Port=5432;Database=KanbanBoard;User Id=postgres;Password=postgres1234;";
        public TestFixture()
        {
            Factory = new WebApplicationFactory<KanbanBoard.WebApi.Startup>();

        }

        public WebApplicationFactory<KanbanBoard.WebApi.Startup> Factory { get; }

        public void Dispose()
        {
            Factory.Dispose();
        }

        public async Task CleanDatabase()
        {
            using IDbConnection connection =
                new NpgsqlConnection(ConnectionString);
            await connection.ExecuteAsync(@"delete from assignments;
            delete from listTasks;
            delete from tasks;
            delete from lists;
            delete from boardMembers;
            delete from users;
            delete from boards;");
        }
    }
}
