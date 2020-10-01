using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.IntegrationTests.WebApi.Helpers;
using KanbanBoard.WebApi.V1.ViewModels;
using Xunit;

namespace KanbanBoard.IntegrationTests.WebApi.V1.Controllers.UsersController
{
    [Trait("TestType", "Integration")]
    [Trait("Category", "UsersController")]
    public class ShowTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public ShowTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task FetchUserByIdShouldResponseWithOkWhenUserExists()
        {
            await _fixture.CleanDatabase();
            UserViewModel user = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "emal@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"api/v1/users/{user.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task FetchUserByIdShouldReturnUserObjectWhenUserExists()
        {
            await _fixture.CleanDatabase();
            UserViewModel user = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "emal@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"api/v1/users/{user.Id}");
            UserViewModel fetchedUser = await response.DeserializeAsAsync<UserViewModel>();

            fetchedUser.Should().NotBeNull();
        }

        [Fact]
        public async Task FetchUserByIdShouldReturnTheUserDataWhenUserExists()
        {
            await _fixture.CleanDatabase();
            UserViewModel user = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "emal@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync($"api/v1/users/{user.Id}");
            UserViewModel fetchedUser = await response.DeserializeAsAsync<UserViewModel>();

            fetchedUser.Id.Should().Be(user.Id);
            fetchedUser.Name.Should().Be(user.Name);
            fetchedUser.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task FetchUserByIdShouldResponseWithNotFoundWhenUserNotExists()
        {
            await _fixture.CleanDatabase();
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync("api/v1/users/1");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
