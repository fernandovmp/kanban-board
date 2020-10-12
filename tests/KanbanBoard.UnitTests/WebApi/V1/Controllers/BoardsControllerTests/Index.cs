using System.Collections.Generic;
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
    public class IndexTests : BoardsControllerTestsBase
    {

        [Fact]
        public async Task IndexShouldReturnOkWhenSuccess()
        {
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task IndexShouldReturnListOfBoardsWhenSuccess()
        {
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<IEnumerable<BoardViewModel>>();
        }

        [Fact]
        public async Task IndexShouldReturnFilledListWhenUserAreMemberOfBoards()
        {
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<IEnumerable<BoardViewModel>>()
                .Should()
                .NotBeNullOrEmpty();
        }

        [Fact]
        public async Task IndexShouldReturnEmptyListWhenUserAreNotMemberOfBoards()
        {
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

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<IEnumerable<BoardViewModel>>()
                .Should()
                .BeEmpty();
        }
    }
}
