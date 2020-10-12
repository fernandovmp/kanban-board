using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.AssignmentsControllerTests
{
    [Trait("Category", "AssignmentsController")]
    public class DeleteTests : AssignmentsControllerTestsBase
    {
        private const int ExistentBoardId = 1;
        private const int NonExistentBoardId = 10;
        private const int ExistentTaskId = 1;
        private const int NonExistentTaskId = 10;
        private const int ExistentMemberIdWithNoAssignments = 2;
        private const int ExistentMemberIdWithAssignment = 1;
        private const int NonExistentMemberId = 10;

        [Fact]
        public async Task ShouldReturnNoContentWhenSuccess()
        {
            int boardId = ExistentBoardId;
            int taskId = ExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenMemberIsNotAssignedToTheTask()
        {
            int boardId = ExistentBoardId;
            int taskId = ExistentTaskId;
            int memberId = ExistentMemberIdWithNoAssignments;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenTaskNotExists()
        {
            int boardId = ExistentBoardId;
            int taskId = NonExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenTaskNotExists()
        {
            int boardId = ExistentBoardId;
            int taskId = NonExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeAssignableTo<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenTaskNotExists()
        {
            int boardId = ExistentBoardId;
            int taskId = NonExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .As<ErrorViewModel>()
                .Status
                .Should()
                .Be(404);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int taskId = ExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentBoardId;
            int taskId = ExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

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
            int taskId = ExistentTaskId;
            int memberId = ExistentMemberIdWithAssignment;
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

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
            int memberId = ExistentMemberIdWithAssignment;
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "10");
            var assignmentsController = new AssignmentsController(
                fakeBoardRepository,
                fakeMemberRepository,
                fakeTaskRepository,
                fakeAssignmentRepository)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult result = await assignmentsController.Delete(boardId, taskId, memberId);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
