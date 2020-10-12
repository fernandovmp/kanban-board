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
    public class CreateTests : BoardsControllerTestsBase
    {
        private readonly PostBoardViewModel _validPostBoardViewModel;

        public CreateTests() : base()
        {
            _validPostBoardViewModel = new PostBoardViewModel
            {
                Title = "project board",
            };
        }

        [Fact]
        public async Task ShouldReturnCreatedAtActionWhenSuccess()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task ShouldReturnBoardViewModelWhenSuccess()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
        public async Task ShouldReturnBoardWithIdWhenSuccess()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<BoardViewModel>()
                .Id
                .Should()
                .NotBe(0);
        }
    }
}
