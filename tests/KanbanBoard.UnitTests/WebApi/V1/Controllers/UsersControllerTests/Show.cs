using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.UsersControllerTests
{
    [Trait("Category", "UsersController")]
    public class ShowTests
    {
        private readonly IUserRepository _fakeUserRepository;
        private const int ExistentUserId = 1;
        private const int NonExistentUserId = 10;

        public ShowTests()
        {
            _fakeUserRepository = new FakeUserRepository();
        }

        [Fact]
        public async Task ShouldReturnOkWhenSuccess()
        {
            int userId = ExistentUserId;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Show(userId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnUserViewModelWhenSuccess()
        {
            int userId = ExistentUserId;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Show(userId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeOfType<UserViewModel>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUserDoesntExists()
        {
            int userId = NonExistentUserId;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Show(userId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
