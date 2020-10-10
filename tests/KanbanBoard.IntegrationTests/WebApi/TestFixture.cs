using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Docker.DotNet;
using KanbanBoard.IntegrationTests.WebApi.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit;

namespace KanbanBoard.IntegrationTests.WebApi
{
    public class TestFixture : IAsyncLifetime
    {
        private DockerClient DockerClient { get; set; }
        private PostgresContainer PostgresContainer { get; set; }

        public WebApplicationFactory<KanbanBoard.WebApi.Startup> Factory { get; private set; }

        public async Task InitializeAsync()
        {
            try
            {
                DockerClient = GetDockerClient();
                PostgresContainer = new PostgresContainer();
                await PostgresContainer.Start(DockerClient);
                Factory = new WebApplicationFactory<KanbanBoard.WebApi.Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder
                            .ConfigureAppConfiguration((context, configurationBuilder) =>
                            {
                                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
                                {
                                    ["ConnectionStrings:PostgresConnection"] = PostgresContainer.ConnectionString
                                });
                            });
                    });
            }
            catch (Exception)
            {
                await PostgresContainer.Remove(DockerClient);
                throw;
            }

        }

        private DockerClient GetDockerClient()
        {
            string dockerUri = IsRunningOnWindows() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock";
            return new DockerClientConfiguration(new Uri(dockerUri))
                .CreateClient();
        }

        private bool IsRunningOnWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

        public async Task DisposeAsync()
        {
            Factory.Dispose();
            await PostgresContainer.Remove(DockerClient);
            DockerClient.Dispose();
        }

        public async Task CleanDatabase()
        {
            using IDbConnection connection =
                new NpgsqlConnection(PostgresContainer.ConnectionString);
            await connection.ExecuteAsync(@"delete from assignments;
            delete from list_tasks;
            delete from tasks;
            delete from lists;
            delete from board_members;
            delete from boards;
            delete from users;");
        }
    }
}
