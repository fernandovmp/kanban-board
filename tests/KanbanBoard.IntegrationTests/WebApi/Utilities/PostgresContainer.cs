using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Npgsql;

namespace KanbanBoard.IntegrationTests.WebApi.Utilities
{
    public class PostgresContainer : DockerContainerBase
    {
        private const string ImageName = "kanban-db";
        private const string HostIp = "localhost";
        private const string PostgresPassword = "postgres1234";

        public PostgresContainer() : base(ImageName, $"/{Guid.NewGuid()}")
        {
        }

        public string ListeningPort { get; private set; }

        public string ConnectionString =>
            $"Host={HostIp};Port={ListeningPort};Database=KanbanBoard;User Id=postgres;Password={PostgresPassword};";

        protected override Config GetConfig() => new Config
        {
            Env = new List<string> { $"POSTGRES_PASSWORD={PostgresPassword}", "POSTGRES_DB=KanbanBoard" }
        };
        protected override HostConfig GetHostConfig()
        {
            ListeningPort = GetFreePort();
            return new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
            {
                {
                    "5432/tcp",
                    new List<PortBinding>
                    {
                        new PortBinding
                        {
                            HostPort = ListeningPort
                        }
                    }
                }
            }
            };
        }

        private static string GetFreePort()
        {
            // Taken from https://stackoverflow.com/a/150974/4190785
            var tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            tcpListener.Stop();
            return port.ToString();
        }

        protected override async Task<bool> IsReady()
        {
            try
            {
                using NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                await connection.OpenAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
