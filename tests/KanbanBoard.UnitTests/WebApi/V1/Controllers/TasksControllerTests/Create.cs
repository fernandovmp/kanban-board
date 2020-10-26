using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes.Repositories;
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
    public class CreateTests : TaskControllerTestsBase
    {
        private readonly PostTaskViewModel _validPostTaskViewModel;
        private readonly IAssignmentRepository _fakeAssignmentRepository;

        public CreateTests() : base()
        {
            _validPostTaskViewModel = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            _fakeAssignmentRepository = new FakeAssignmentRepository();
        }

        [Fact]
        public async Task ShouldReturnCreatedAtWhenSuccess()
        {
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 1;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task ShouldReturnKanbanTaskViewModelWhenSuccess()
        {
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 1;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .Should()
                .BeOfType<KanbanTaskViewModel>();
        }

        [Fact]
        public async Task ShouldReturnKanbanTaskWithIdWhenSuccess()
        {
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 1;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<KanbanTaskViewModel>()
                .Id
                .Should()
                .NotBe(0);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 10;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 10;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

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
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 10;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

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
            PostTaskViewModel model = _validPostTaskViewModel;
            int boardId = 1;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

            result.Result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task CreateShouldReturnNotFoundWhenNotFoundTheListOnTheBoard()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 10,
                TagColor = "FFFFFF"
            };
            int boardId = 1;
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

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId, _fakeAssignmentRepository);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
