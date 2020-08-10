using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers
{
    [Trait("Category", "BoardsController")]
    public class BoardsControllerTests
    {

        [Fact]
        public async Task CreateShouldReturnCreatedAtActionWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "project board",
            };

            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.Insert(It.IsAny<Board>()))
                .ReturnsAsync((Board board) => new Board
                {
                    Id = 1,
                    Title = board.Title,
                    CreatedBy = board.CreatedBy,
                    CreatedOn = board.CreatedOn,
                    ModifiedOn = board.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateShouldReturnBoardViewModelWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "project board",
            };

            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.Insert(It.IsAny<Board>()))
                .ReturnsAsync((Board board) => new Board
                {
                    Id = 1,
                    Title = board.Title,
                    CreatedBy = board.CreatedBy,
                    CreatedOn = board.CreatedOn,
                    ModifiedOn = board.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .Should()
                .BeOfType<BoardViewModel>();
        }

        [Fact]
        public async Task CreateShouldReturnBoardWithIdWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "project board",
            };
            int expectedId = 1;

            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.Insert(It.IsAny<Board>()))
                .ReturnsAsync((Board board) => new Board
                {
                    Id = expectedId,
                    Title = board.Title,
                    CreatedBy = board.CreatedBy,
                    CreatedOn = board.CreatedOn,
                    ModifiedOn = board.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<BoardViewModel>()
                .Id
                .Should()
                .Be(expectedId);
        }

        private ControllerContext GetFakeControlerContextWithFakeUser(string identityName)
        {
            var fakeHttpContext = new Mock<HttpContext>();
            fakeHttpContext
                .Setup(httpContext => httpContext.User.Identity.Name)
                .Returns(identityName);
            var fakeControllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };
            return fakeControllerContext;
        }
        /*
                [Fact]
                public async Task CreateShouldReturnUserWithAnIdWhenSuccess()
                {
                    int expectedId = 1;
                    var model = new SignUpViewModel
                    {
                        Name = "default",
                        Email = "email@example.com",
                        Password = "password",
                        ConfirmPassword = "password"
                    };
                    var fakeUserRepository = new Mock<IUserRepository>();
                    fakeUserRepository
                        .Setup(repository => repository.Insert(It.IsAny<User>()))
                        .ReturnsAsync((User user) => new User
                        {
                            Id = expectedId,
                            Name = user.Name,
                            Email = user.Email,
                            Password = user.Password
                        });
                    fakeUserRepository
                        .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                        .Returns(Task.FromResult(false));
                    var fakePasswordHasher = new Mock<IPasswordHasherService>();
                    fakePasswordHasher
                        .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                        .Returns("HashedPassword");
                    var usersController = new UsersController(
                        fakePasswordHasher.Object,
                        fakeUserRepository.Object);

                    ActionResult<UserViewModel> result = await usersController.Create(model);

                    result
                        .Result
                        .As<CreatedAtActionResult>()
                        .Value
                        .As<UserViewModel>()
                        .Id
                        .Should()
                        .Be(expectedId);
                }

                [Fact]
                public async Task CreateShouldReturnConflictWhenEmailIsAlreadyInUse()
                {
                    var model = new SignUpViewModel
                    {
                        Name = "default",
                        Email = "email@example.com",
                        Password = "password",
                        ConfirmPassword = "password"
                    };
                    var fakeUserRepository = new Mock<IUserRepository>();
                    fakeUserRepository
                        .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                        .Returns(Task.FromResult(true));
                    var fakePasswordHasher = new Mock<IPasswordHasherService>();
                    var usersController = new UsersController(
                        fakePasswordHasher.Object,
                        fakeUserRepository.Object);

                    ActionResult<UserViewModel> result = await usersController.Create(model);

                    result
                        .Result
                        .Should()
                        .BeOfType<ConflictObjectResult>();
                }

                [Fact]
                public async Task CreateShouldReturnErrorViewModelWhenEmailIsAlreadyInUse()
                {
                    var model = new SignUpViewModel
                    {
                        Name = "default",
                        Email = "email@example.com",
                        Password = "password",
                        ConfirmPassword = "password"
                    };
                    var fakeUserRepository = new Mock<IUserRepository>();
                    fakeUserRepository
                        .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                        .Returns(Task.FromResult(true));
                    var fakePasswordHasher = new Mock<IPasswordHasherService>();
                    var usersController = new UsersController(
                        fakePasswordHasher.Object,
                        fakeUserRepository.Object);

                    ActionResult<UserViewModel> result = await usersController.Create(model);

                    result
                        .Result
                        .As<ConflictObjectResult>()
                        .Value
                        .Should()
                        .BeOfType<ErrorViewModel>();
                }*/
    }
}
