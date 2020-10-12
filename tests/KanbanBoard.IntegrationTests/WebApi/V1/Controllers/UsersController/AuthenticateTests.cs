using System.Linq;
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
    public class AuthenticateTests : IClassFixture<TestFixture> //: IClassFixture<TestFixture>
    {

        private readonly TestFixture _fixture;

        public AuthenticateTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AuthenticationShouldBeSuccessfulWhenCredendialsAreValid()
        {
            await _fixture.CleanDatabase();
            _ = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "Password"
            };
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync($"api/v1/login", model);

            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task AuthenticationShouldReturnJwtWhenSuccess()
        {
            await _fixture.CleanDatabase();
            _ = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "Password"
            };
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync($"api/v1/login", model);
            LogInResponseViewModel loginData = await response.DeserializeAsAsync<LogInResponseViewModel>();

            loginData.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task AuthenticationShouldResponseWithBadRequestWhenCredendialsAreInvalid()
        {
            await _fixture.CleanDatabase();
            _ = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "DistinctPassword"
            };
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync($"api/v1/login", model);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AuthenticationShouldPointErrorOnPasswordWhenCredendialsAreInvalid()
        {
            await _fixture.CleanDatabase();
            _ = await UserHelpers.CreateUser(_fixture, new SignUpViewModel
            {
                Email = "email@example.com",
                Name = "Name",
                Password = "Password",
                ConfirmPassword = "Password"
            });
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "DistinctPassword"
            };
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync($"api/v1/login", model);
            ValidationErrorViewModel error = await response.DeserializeAsAsync<ValidationErrorViewModel>();

            error
                .Errors
                .Select(err => err.Property == nameof(LogInViewModel.Password))
                .Should()
                .HaveCountGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task AuthenticationShouldResponseWithNotFoundWhenUserNotExists()
        {
            await _fixture.CleanDatabase();
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "DistinctPassword"
            };
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync($"api/v1/login", model);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
