using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Mocks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers
{
    public class TasksControllerTests : ControllerTestsBase
    {
        [Fact]
        public async Task CreateShouldReturnCreatedAtWhenSuccess()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: true)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateShouldReturnKanbanTaskViewModelWhenSuccess()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: true)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .Should()
                .BeOfType<KanbanTaskViewModel>();
        }

        [Fact]
        public async Task CreateShouldReturnKanbanTaskWithIdWhenSuccess()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;
            int expectedId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: true)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: expectedId);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<KanbanTaskViewModel>()
                .Id
                .Should()
                .Be(expectedId);
        }

        [Fact]
        public async Task CreateShouldReturnNotFoundWhenBoardNotExists()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: false)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task CreateShouldReturnErrorViewModelWhenBoardNotExists()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: false)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task CreateShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: false)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

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
        public async Task CreateShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: true)
                .MockGetBoardMember(returnValue: null)
                .MockGetBoardList(new KanbanList
                {
                    Id = 1,
                    Board = new Board
                    {
                        Id = boardId
                    },
                    Title = "List Title"
                })
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result.Result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task CreateShouldReturnForbidWhenListNotBelongsToTheBoard()
        {
            var model = new PostTaskViewModel
            {
                Summary = "Task",
                Description = "Important Task",
                AssignedTo = new List<int>(),
                List = 1,
                TagColor = "FFFFFF"
            };
            int boardId = 1;

            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: true)
                .MockGetBoardMember(isAdmin: false)
                .MockGetBoardList(returnValue: null)
                .MockInsertKanbanTask(withId: 1);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            ControllerContext context = GetFakeControlerContextWithFakeUser(identityName: "1");

            var tasksController = new TasksController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = context,
                Url = fakeUrlHelper
            };

            ActionResult<KanbanTaskViewModel> result = await tasksController.Create(model, boardId);

            result.Result.Should().BeOfType<ForbidResult>();
        }
    }
}
