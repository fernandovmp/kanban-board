using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.BoardsControllerTests
{
    [Trait("Category", "BoardsController")]
    public class ShowTests : ControllerTestsBase
    {
        private readonly IBoardRepository _fakeBoardRepository;
        private readonly IUrlHelper _fakeUrlHelper;
        private readonly ControllerContext _fakeControllerContext;
        private const int ExistentId = 1;
        private const int NonExistentId = 10;

        public ShowTests()
        {
            _fakeBoardRepository = new FakeBoardRepository();
            _fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            _fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }

        [Fact]
        public async Task ShowShouldReturnOkWhenSuccess()
        {
            int boardId = ExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShowShouldReturnDetailedBoardViewModelWhenSuccess()
        {
            int boardId = ExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeOfType<DetailedBoardViewModel>();
        }

        [Fact]
        public async Task ShowShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShowShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task ShowShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

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
        public async Task ShowShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            int boardId = ExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "10");
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = _fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result.Result.Should().BeOfType<ForbidResult>();
        }
    }
}
