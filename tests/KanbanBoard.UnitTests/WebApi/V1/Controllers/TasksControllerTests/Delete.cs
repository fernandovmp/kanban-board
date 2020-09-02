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

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.TasksControllerTests
{
    [Trait("Category", "TasksController")]
    public class DeleteTests : ControllerTestsBase
    {
        private readonly IBoardRepository _fakeBoardRepository;
        private readonly IUrlHelper _fakeUrlHelper;
        private readonly ControllerContext _controllerContext;
        private const int ExistentBoardId = 1;
        private const int NonExistentBoardId = 10;
        private const int ExistentTaskId = 1;
        private const int NonExistentTaskId = 10;

        public DeleteTests()
        {
            _fakeBoardRepository = new FakeBoardRepository();
            _fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            _controllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenSuccess()
        {
            int boardId = ExistentBoardId;
            int taskId = ExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int taskId = ExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int taskId = ExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

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
            int taskId = ExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

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
            int taskId = ExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "10");

            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenTaskNotExists()
        {
            int boardId = ExistentTaskId;
            int taskId = NonExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenTaskNotExists()
        {
            int boardId = ExistentTaskId;
            int taskId = NonExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenTaskNotExists()
        {
            int boardId = ExistentTaskId;
            int taskId = NonExistentTaskId;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                _fakeBoardRepository,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = _controllerContext,
                Url = _fakeUrlHelper
            };

            ActionResult result = await tasksController.Delete(boardId, taskId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .As<ErrorViewModel>()
                .Status
                .Should()
                .Be(404);
        }
    }
}
