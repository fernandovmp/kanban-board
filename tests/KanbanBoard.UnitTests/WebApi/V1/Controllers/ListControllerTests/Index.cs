using System.Collections.Generic;
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
    public class IndexTests : ListsControllerTestsBase
    {
        private const int ExistentBoardId = 1;
        private const int NonExistentBoardId = 10;

        [Fact]
        public async Task ShouldReturnOkWhenSuccess()
        {
            int boardId = ExistentBoardId;
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

            ActionResult<IEnumerable<KanbanListViewModel>> result = await listsController.Index(boardId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
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

            ActionResult<IEnumerable<KanbanListViewModel>> result = await listsController.Index(boardId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
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

            ActionResult<IEnumerable<KanbanListViewModel>> result = await listsController.Index(boardId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
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

            ActionResult<IEnumerable<KanbanListViewModel>> result = await listsController.Index(boardId);

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
            int boardId = ExistentBoardId;
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

            ActionResult<IEnumerable<KanbanListViewModel>> result = await listsController.Index(boardId);

            result.Result.Should().BeOfType<ForbidResult>();
        }
    }
}
