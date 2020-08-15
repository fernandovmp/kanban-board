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

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.BoardsControllerTests
{
    [Trait("Category", "BoardsController")]
    public class IndexTests : ControllerTestsBase
    {
        private readonly IBoardRepository _fakeBoardRepository;
        private readonly IUrlHelper _fakeUrlHelper;
        private readonly ControllerContext _fakeControllerContext;

        public IndexTests()
        {
            _fakeBoardRepository = new FakeBoardRepository();
            _fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            _fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }

        [Fact]
        public async Task IndexShouldReturnOkWhenSuccess()
        {
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task IndexShouldReturnListOfBoardsWhenSuccess()
        {
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardsController = new BoardsController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _fakeControllerContext,
                Url = _fakeUrlHelper
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
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = _fakeUrlHelper
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
