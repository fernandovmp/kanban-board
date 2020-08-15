using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.UnitTests.WebApi.Mocks;
using KanbanBoard.WebApi.Models;
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
    public class UpdateTests : ControllerTestsBase
    {
        private readonly IBoardRepository _fakeBoardRepository;
        private readonly IUrlHelper _fakeUrlHelper;
        private readonly ControllerContext _fakeControllerContext;
        private readonly PostBoardViewModel _validPostBoardViewModel;
        private const int ExistentId = 1;
        private const int NonExistentId = 10;

        public UpdateTests()
        {
            _fakeBoardRepository = new FakeBoardRepository();
            _fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            _fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            _validPostBoardViewModel = new PostBoardViewModel
            {
                Title = "project board",
            };
        }

        [Fact]
        public async Task UpdateShouldReturnNoContentWhenSuccess()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            int boardId = ExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnNotFoundWhenBoardNotExists()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnErrorViewModelWhenBoardNotExists()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task UpdateShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
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

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnForbidWhenUserIsNotBoardAdmin()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: true)
                .MockGetBoardMember(isAdmin: false);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
