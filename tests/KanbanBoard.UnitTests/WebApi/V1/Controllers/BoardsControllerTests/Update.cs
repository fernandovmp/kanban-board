using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.BoardsControllerTests
{
    [Trait("Category", "BoardsController")]
    public class UpdateTests : BoardsControllerTestsBase
    {
        private readonly PostBoardViewModel _validPostBoardViewModel;
        private const int ExistentId = 1;
        private const int NonExistentId = 10;

        public UpdateTests() : base()
        {
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnForbidWhenUserIsNotBoardAdmin()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            int boardId = 1;
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "2");
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
