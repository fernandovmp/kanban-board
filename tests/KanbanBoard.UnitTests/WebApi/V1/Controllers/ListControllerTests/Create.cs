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

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.ListsControllerTests
{
    [Trait("Category", "ListsController")]
    public class CreateTests : ControllerTestsBase
    {
        private readonly IBoardRepository _fakeBoardRepository;
        private readonly IUrlHelper _fakeUrlHelper;
        private readonly ControllerContext _fakeControllerContext;
        private readonly PostListViewModel _validPostListViewModel;

        public CreateTests()
        {
            _fakeBoardRepository = new FakeBoardRepository();
            _fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            _fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            _validPostListViewModel = new PostListViewModel
            {
                Title = "default",
            };
        }

        [Fact]
        public async Task ShouldReturnCreatedAtActionWhenSuccess()
        {
            int boardId = 1;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task ShouldReturnKanbanListViewModelWhenSuccess()
        {
            int boardId = 1;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
        public async Task ShouldReturnListWithIdWhenSuccess()
        {
            int boardId = 1;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<KanbanListViewModel>()
                .Id
                .Should()
                .NotBe(0);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = 10;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .Should()
                .BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = 10;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
        public async Task ShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = 10;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
        public async Task ShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            int boardId = 1;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            ControllerContext controllerContext = GetFakeControlerContextWithFakeUser(identityName: "10");
            var listsController = new ListsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .Should()
                .BeOfType<ForbidResult>();
        }
    }
}
