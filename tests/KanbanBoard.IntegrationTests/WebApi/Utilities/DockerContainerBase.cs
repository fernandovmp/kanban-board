using System;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using FluentAssertions.Extensions;

namespace KanbanBoard.IntegrationTests.WebApi.Utilities
{
    public abstract class DockerContainerBase
    {

        private readonly string _imageName;
        private readonly string _containerName;

        protected DockerContainerBase(string imageName, string containerName)
        {
            _imageName = imageName;
            _containerName = containerName;
        }

        public async Task Start(IDockerClient dockerClient)
        {
            await CreateContainerAsync(dockerClient);
            bool started = await StartContainerAsync(dockerClient);
            if (!started)
            {
                throw new InvalidOperationException($"Container '{_containerName}' did not start!");
            }
            await ContainerIsReadyAsync();
        }

        private Task<bool> StartContainerAsync(IDockerClient dockerClient)
        {
            return dockerClient.Containers.StartContainerAsync(_containerName, new ContainerStartParameters());
        }

        private async Task CreateContainerAsync(IDockerClient dockerClient)
        {
            Config config = GetConfig();
            HostConfig hostConfig = GetHostConfig();
            await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters(config)
            {
                Image = _imageName,
                Name = _containerName,
                Tty = true,
                HostConfig = hostConfig
            });
        }

        protected abstract Config GetConfig();
        protected abstract HostConfig GetHostConfig();

        private async Task ContainerIsReadyAsync()
        {
            int i = 0;
            while (!await IsReady())
            {
                i++;
                if (i > 20)
                {
                    throw new TimeoutException(
                       $"Container {_containerName} does not seem to be responding in a timely manner");
                }
                await Task.Delay(5.Seconds());
            }
        }

        protected abstract Task<bool> IsReady();

        public Task Remove(IDockerClient dockerClient)
        {
            return dockerClient.Containers.RemoveContainerAsync(_containerName,
                new ContainerRemoveParameters { Force = true, RemoveVolumes = true });
        }
    }
}
