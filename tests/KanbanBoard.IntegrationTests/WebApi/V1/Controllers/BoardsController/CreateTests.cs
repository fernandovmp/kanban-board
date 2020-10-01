using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.IntegrationTests.WebApi.Helpers;
using KanbanBoard.WebApi.V1.ViewModels;
using Xunit;

namespace KanbanBoard.IntegrationTests.WebApi.V1.Controllers.BoardsController
{
    [Trait("TestType", "Integration")]
    [Trait("Category", nameof(KanbanBoard.WebApi.V1.Controllers.BoardsController))]
    public class CreateTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly AuthenticationData _defaultAuthenticationData = new AuthenticationData
        {
            Name = "Test User",
            Email = "email@example.com",
            Password = "Password"
        };

        public CreateTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(ValidPostBoardData))]
        public async Task CreateShouldResponseWithCreatedWhenSendValidData(PostBoardViewModel model)
        {
            await _fixture.CleanDatabase();
            string token = await AuthenticationHelpers.GetToken(_fixture, _defaultAuthenticationData);
            using HttpClient client = _fixture.Factory.CreateClient();
            client.SetBearerToken(token);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/boards", model);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [MemberData(nameof(ValidPostBoardData))]
        public async Task CreateShouldReturnBoardDataWhenSendValidData(PostBoardViewModel model)
        {
            await _fixture.CleanDatabase();
            string token = await AuthenticationHelpers.GetToken(_fixture, _defaultAuthenticationData);
            using HttpClient client = _fixture.Factory.CreateClient();
            client.SetBearerToken(token);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/boards", model);
            BoardViewModel createdBoard = await response.DeserializeAsAsync<BoardViewModel>();

            createdBoard.Id.Should().BeGreaterThan(0);
            createdBoard.Title.Should().Be(model.Title);
        }

        [Theory]
        [MemberData(nameof(ValidPostBoardData))]
        public async Task FetchTheLocationHeaderShouldResponseWithOk(PostBoardViewModel model)
        {
            await _fixture.CleanDatabase();
            string token = await AuthenticationHelpers.GetToken(_fixture, _defaultAuthenticationData);
            using HttpClient client = _fixture.Factory.CreateClient();
            client.SetBearerToken(token);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/boards", model);
            HttpResponseMessage locationResponse = await client.GetAsync(response.Headers.Location);

            locationResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static IEnumerable<object[]> ValidPostBoardData()
        {
            yield return new[] {
                new PostBoardViewModel {
                    Title = "Untitled"
                }
            };
            yield return new[] {
                new PostBoardViewModel {
                    Title = "Board #1"
                }
            };
            yield return new[] {
                new PostBoardViewModel {
                    Title = "Board Title with 150 Character spXJXWbBW9YlIOmbAtFkBXnzpVNkhuT3QnCCXbfB6CHRztkZDqCBHIXUJDp8w1119dckmcWLHnCbExEjyOzjoXIxCYiHqmjogxfFQR5y43YWG7GgzyTNXP6"
                }
            };
        }

        [Fact]
        public async Task CreateShouldResponseWithUnauthorizedWhenNotProvideAuthenticationToken()
        {
            await _fixture.CleanDatabase();
            var model = new PostBoardViewModel
            {
                Title = "Board #1"
            };
            using HttpClient client = _fixture.Factory.CreateClient();

            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/boards", model);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(InvalidPostBoardData))]
        public async Task CreateShouldResponseWithBadRequestWhenSendInvalidData(PostBoardViewModel model)
        {
            await _fixture.CleanDatabase();
            string token = await AuthenticationHelpers.GetToken(_fixture, _defaultAuthenticationData);
            using HttpClient client = _fixture.Factory.CreateClient();
            client.SetBearerToken(token);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/boards", model);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(InvalidPostBoardData))]
        public async Task CreateShouldPointErrorToTitleWhenSendInvalidData(PostBoardViewModel model)
        {
            await _fixture.CleanDatabase();
            string token = await AuthenticationHelpers.GetToken(_fixture, _defaultAuthenticationData);
            using HttpClient client = _fixture.Factory.CreateClient();
            client.SetBearerToken(token);

            HttpResponseMessage response = await client.PostAsJsonAsync("api/v1/boards", model);
            ValidationErrorViewModel errorData = await response.DeserializeAsAsync<ValidationErrorViewModel>();

            errorData
                .Errors
                .Select(error => error.Property == nameof(PostBoardViewModel.Title))
                .Should()
                .HaveCountGreaterThan(0);
        }

        public static IEnumerable<object[]> InvalidPostBoardData()
        {
            yield return new[] {
                new PostBoardViewModel {
                    Title = ""
                }
            };
            yield return new[] {
                new PostBoardViewModel {
                    Title = null
                }
            };
            yield return new[] {
                new PostBoardViewModel {
                    Title = "Board Title with more than 150 Character spXJXWbBW9YlIOmbAtFkBXnzpVNkhuT3QnCCXbfB6CHRztkZDqCBHIXUJDp8w1119dckmcWLHnCbExEjyOzjoXIxCYiHqmjogxfFQR5y43YWG7GgzyTNXP6"
                }
            };
        }
    }
}
