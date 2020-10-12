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
    public class DeleteTests : ListsControllerTestsBase
    {
        private const int ExistentBoardId = 1;
        private const int ExistentListId = 1;
        private const int NonExistentBoardId = 10;

        [Fact]
        public async Task ShouldReturnNoContentWhenSuccess()
        {
            int boardId = ExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Delete(boardId, listId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Delete(boardId, listId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int listId = ExistentListId;
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

            ActionResult result = await listsController.Delete(boardId, listId);

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

            ActionResult result = await listsController.Delete(boardId, listId);

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

            ActionResult result = await listsController.Delete(boardId, listId);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
