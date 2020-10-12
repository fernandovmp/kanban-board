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
    public class ShowTests : TaskControllerTestsBase
    {
        public ShowTests() : base()
        {
        }

        [Fact]
        public async Task ShouldReturnOkWhenSuccess()
        {
            int boardId = 1;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnBoardTaskViewModelWhenSuccess()
        {
            int boardId = 1;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeOfType<BoardTaskViewModel>();
        }

        [Fact]
        public async Task ShouldNotReturnNullViewModelWhenSuccess()
        {
            int boardId = 1;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<BoardTaskViewModel>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task ShouldReturnBoardTaskViewModelWithIdWhenSuccess()
        {
            int boardId = 1;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<BoardTaskViewModel>()
                .Id
                .Should()
                .Be(1);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = 10;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = 10;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

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
            int boardId = 10;
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

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
            int taskId = 1;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "10");

            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result.Result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenTaskNotExists()
        {
            int boardId = 1;
            int taskId = 10;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenTaskNotExists()
        {
            int boardId = 1;
            int taskId = 10;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenTaskNotExists()
        {
            int boardId = 1;
            int taskId = 10;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var tasksController = new TasksController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeTaskRepository,
                fakeListRepository,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<BoardTaskViewModel> result = await tasksController.Show(boardId, taskId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .As<ErrorViewModel>()
                .Status
                .Should()
                .Be(404);
        }
    }
}
