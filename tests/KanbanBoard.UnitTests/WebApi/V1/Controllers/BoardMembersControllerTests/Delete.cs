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

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.BoardMembersControllerTests
{
    [Trait("Category", "BoardMembersController")]
    public class DeleteTests : BoardMembersControllerTestsBase
    {
        private const int ExistentBoardId = 1;
        private const int NonExistentBoardId = 10;
        private const int ExistentMemberId = 1;
        private const int NonExistentMemberId = 10;

        [Fact]
        public async Task ShouldReturnNoContentWhenSuccess()
        {
            int boardId = ExistentBoardId;
            int memberId = ExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenRequestedMemberToRemoveIsNotMemberOfTheBoard()
        {
            int boardId = ExistentBoardId;
            int memberId = NonExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int memberId = ExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int memberId = ExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int memberId = ExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

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
            int memberId = ExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "10");
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task ShouldReturnForbidWhenUserIsNotAdminOfTheBoard()
        {
            int boardId = ExistentBoardId;
            int memberId = ExistentMemberId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "2");
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Delete(boardId, memberId);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
