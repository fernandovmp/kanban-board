using System.Collections.Generic;
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
    public class IndexTests : BoardMembersControllerTestsBase
    {
        private const int ExistentId = 1;
        private const int NonExistentId = 10;

        [Fact]
        public async Task ShouldReturnOkWhenSuccess()
        {
            int boardId = ExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardMemberViewModel>> result = await boardMembersController.Index(boardId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnListOfBoardMembersWhenSuccess()
        {
            int boardId = ExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardMemberViewModel>> result = await boardMembersController.Index(boardId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardMemberViewModel>> result = await boardMembersController.Index(boardId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardMemberViewModel>> result = await boardMembersController.Index(boardId);

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
            int boardId = NonExistentId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardMemberViewModel>> result = await boardMembersController.Index(boardId);

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
            int boardId = ExistentId;
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

            ActionResult<IEnumerable<BoardMemberViewModel>> result = await boardMembersController.Index(boardId);

            result.Result.Should().BeOfType<ForbidResult>();
        }
    }
}
