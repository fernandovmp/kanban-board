using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.ListsControllerTests
{
    [Trait("Category", "ListsController")]
    public class CreateTests : ListsControllerTestsBase
    {
        private readonly PostListViewModel _validPostListViewModel;

        public CreateTests() : base()
        {
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
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
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = controllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanListViewModel> result = await listsController.Create(model, boardId);

            result
                .Result
                .Should()
                .BeOfType<ForbidResult>();
        }
    }
}
