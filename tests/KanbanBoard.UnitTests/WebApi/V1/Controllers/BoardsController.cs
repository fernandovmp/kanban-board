using System.Collections.Generic;
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

        [Fact]
        public async Task IndexShouldReturnOkWhenSuccess()
        {
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>
                {
                    new Board {
                        Id = 1,
                        Title = "Board #1",
                    },
                    new Board {
                        Id = 2,
                        Title = "Board #2",
                    },
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task IndexShouldReturnListOfBoardsWhenSuccess()
        {
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>
                {
                    new Board {
                        Id = 1,
                        Title = "Board #1",
                    },
                    new Board {
                        Id = 2,
                        Title = "Board #2",
                    },
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<IEnumerable<BoardViewModel>>();
        }

        [Fact]
        public async Task IndexShouldReturnFilledListWhenUserAreMemberOfBoards()
        {
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>
                {
                    new Board {
                        Id = 1,
                        Title = "Board #1",
                    },
                    new Board {
                        Id = 2,
                        Title = "Board #2",
                    },
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<IEnumerable<BoardViewModel>>()
                .Should()
                .NotBeNullOrEmpty();
        }

        [Fact]
        public async Task IndexShouldReturnEmptyListWhenUserAreNotMemberOfBoards()
        {
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>());
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<IEnumerable<BoardViewModel>>()
                .Should()
                .BeEmpty();
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

    }
}
