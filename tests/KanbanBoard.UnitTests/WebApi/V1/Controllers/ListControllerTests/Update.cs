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
    public class UpdateTests : ListsControllerTestsBase
    {
        private readonly PostListViewModel _validPostListViewModel;
        private const int ExistentBoardId = 1;
        private const int ExistentListId = 1;
        private const int NonExistentBoardId = 10;
        private const int NonExistentListId = 10;

        public UpdateTests() : base()
        {
            _validPostListViewModel = new PostListViewModel
            {
                Title = "default",
            };
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenSuccess()
        {
            int boardId = ExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .As<ErrorViewModel>()
                .Status
                .Should()
                .Be(404);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenListNotExists()
        {
            int boardId = ExistentBoardId;
            int listId = NonExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenListNotExists()
        {
            int boardId = ExistentBoardId;
            int listId = NonExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenListNotExists()
        {
            int boardId = ExistentBoardId;
            int listId = NonExistentListId;
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

            ActionResult result = await listsController.Update(model, boardId, listId);

            result
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
            int boardId = ExistentBoardId;
            int listId = ExistentListId;
            PostListViewModel model = _validPostListViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "10");
            var listsController = new ListsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository,
                fakeListRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult result = await listsController.Update(model, boardId, listId);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
