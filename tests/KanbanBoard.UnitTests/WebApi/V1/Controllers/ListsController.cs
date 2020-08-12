using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Mocks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers
{
    [Trait("Category", "ListsController")]
    public class ListsControllerTests : ControllerTestsBase
    {

        [Fact]
        public async Task CreateShouldReturnCreatedAtActionWhenSuccess()
        {
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(isAdmin: false)
                .MockExistsBoard(exists: true)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = 1,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateShouldReturnKanbanListViewModelWhenSuccess()
        {
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(isAdmin: false)
                .MockExistsBoard(exists: true)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = 1,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .Should()
                .BeOfType<KanbanListViewModel>();
        }

        [Fact]
        public async Task CreateShouldReturnListWithIdWhenSuccess()
        {
            int expectedId = 1;
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(isAdmin: false)
                .MockExistsBoard(exists: true)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = expectedId,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<KanbanListViewModel>()
                .Id
                .Should()
                .Be(expectedId);
        }

        [Fact]
        public async Task CreateShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(isAdmin: false)
                .MockExistsBoard(exists: false)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = 1,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .Should()
                .BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(isAdmin: false)
                .MockExistsBoard(exists: false)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = 1,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task CreateShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(isAdmin: false)
                .MockExistsBoard(exists: false)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = 1,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .As<ErrorViewModel>()
                .Status
                .Should()
                .Be(404);
        }

        [Fact]
        public async Task CreateShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            int boardId = 1;
            var model = new PostListViewModel
            {
                Title = "default",
            };
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardMember(null)
                .MockExistsBoard(exists: true)
                .MockInsertKanbanList(list => new KanbanList
                {
                    Id = 1,
                    Title = list.Title,
                    Board = list.Board,
                    CreatedOn = list.CreatedOn,
                    ModifiedOn = list.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var listsController = new ListsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .Should()
                .BeOfType<ForbidResult>();
        }
    }
}
