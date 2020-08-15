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
    public class CreateTests : ControllerTestsBase
    {
        private readonly IBoardRepository _fakeBoardRepository;
        private readonly IUrlHelper _fakeUrlHelper;
        private readonly ControllerContext _fakeControllerContext;
        private readonly PostBoardViewModel _validPostBoardViewModel;

        public CreateTests()
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
        public async Task ShouldReturnCreatedAtActionWhenSuccess()
        {
            PostBoardViewModel model = _validPostBoardViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
