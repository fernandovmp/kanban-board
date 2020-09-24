using System.Collections.Generic;
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
    public class CreateTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public CreateTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldResponseWithStatusCodeCreatedWhenUserIsCreated()
        {
            await _fixture.CleanDatabase();

            using HttpClient client = _fixture.Factory.CreateClient();
            var model = new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password",
            };
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task ShouldReturnUserDataWhenWhenUserIsCreated()
        {
            await _fixture.CleanDatabase();

            using HttpClient client = _fixture.Factory.CreateClient();
            var model = new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password",
            };
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);
            UserViewModel createdUser = await response.DeserializeAsAsync<UserViewModel>();

            createdUser.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturnUserIdWhenUserIsCreated()
        {
            await _fixture.CleanDatabase();

            using HttpClient client = _fixture.Factory.CreateClient();
            var model = new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password",
            };
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);
            UserViewModel createdUser = await response.DeserializeAsAsync<UserViewModel>();

            createdUser.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task FetchTheLocationHeaderShouldBeSuccssWhenUserIsCreated()
        {
            await _fixture.CleanDatabase();

            using HttpClient client = _fixture.Factory.CreateClient();
            var model = new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password",
            };
            HttpResponseMessage postResponse = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);
            HttpResponseMessage locationResponse = await client.GetAsync(postResponse.Headers.Location);

            locationResponse.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task FetchTheLocationHeaderShouldReturnUserWithSameIdAsTheCreated()
        {
            await _fixture.CleanDatabase();

            using HttpClient client = _fixture.Factory.CreateClient();
            var model = new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password",
            };
            HttpResponseMessage postResponse = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);
            UserViewModel createdUser = await postResponse.DeserializeAsAsync<UserViewModel>();

            HttpResponseMessage locationResponse = await client.GetAsync(postResponse.Headers.Location);
            UserViewModel fetchedUserInLocation = await locationResponse.DeserializeAsAsync<UserViewModel>();

            fetchedUserInLocation.Id.Should().Be(createdUser.Id);
        }

        [Fact]
        public async Task ShouldResponseWithConflictWhenEmailIsAlreadyInUse()
        {
            await _fixture.CleanDatabase();

            using HttpClient client = _fixture.Factory.CreateClient();
            var model = new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password",
            };
            _ = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);

            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Theory]
        [MemberData(nameof(InvalidSignUpData))]
        public async Task ShouldResponseWithBadRequestWhenSendInvalidData(SignUpViewModel model)
        {
            await _fixture.CleanDatabase();
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/v1/users", model);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static IEnumerable<object[]> InvalidSignUpData()
        {
            yield return new[]{
                new SignUpViewModel {
                    Email = null,
                    Name = "Name",
                    Password = "Password",
                    ConfirmPassword = "Password"
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "",
                    Name = "Name",
                    Password = "Password",
                    ConfirmPassword = "Password"
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "Email",
                    Name = "Name",
                    Password = "Password",
                    ConfirmPassword = "Password"
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "email@example.com",
                    Name = null,
                    Password = "Password",
                    ConfirmPassword = "Password"
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "email@example.com",
                    Name = "",
                    Password = "Password",
                    ConfirmPassword = "Password"
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "email@example.com",
                    Name = "zoqBWKRookunPrDsLbEAKSUbjCdPkIhkDbREMoNZhTUUNyeVWtQzyRMzJaShPcjdkrNDtIrpGJLXUGaGrTOEnUVOwkuyhduTPLNro",
                    Password = "Password",
                    ConfirmPassword = "Password"
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "email@example.com",
                    Name = "Name",
                    Password = null,
                    ConfirmPassword = null
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "email@example.com",
                    Name = "Name",
                    Password = "",
                    ConfirmPassword = ""
                }
            };
            yield return new[]{
                new SignUpViewModel {
                    Email = "email@example.com",
                    Name = "Name",
                    Password = "Password",
                    ConfirmPassword = "DistinctPassword"
                }
            };
        }
    }
}
