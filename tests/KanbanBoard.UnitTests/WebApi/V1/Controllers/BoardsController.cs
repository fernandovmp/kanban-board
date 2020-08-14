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
    [Trait("Category", "BoardsController")]
    public class BoardsControllerTests : ControllerTestsBase
    {

        [Fact]
        public async Task CreateShouldReturnCreatedAtActionWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "project board",
            };

            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.Insert(It.IsAny<Board>()))
                .ReturnsAsync((Board board) => new Board
                {
                    Id = 1,
                    Title = board.Title,
                    CreatedBy = board.CreatedBy,
                    CreatedOn = board.CreatedOn,
                    ModifiedOn = board.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateShouldReturnBoardViewModelWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "project board",
            };

            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.Insert(It.IsAny<Board>()))
                .ReturnsAsync((Board board) => new Board
                {
                    Id = 1,
                    Title = board.Title,
                    CreatedBy = board.CreatedBy,
                    CreatedOn = board.CreatedOn,
                    ModifiedOn = board.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .Should()
                .BeOfType<BoardViewModel>();
        }

        [Fact]
        public async Task CreateShouldReturnBoardWithIdWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "project board",
            };
            int expectedId = 1;

            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.Insert(It.IsAny<Board>()))
                .ReturnsAsync((Board board) => new Board
                {
                    Id = expectedId,
                    Title = board.Title,
                    CreatedBy = board.CreatedBy,
                    CreatedOn = board.CreatedOn,
                    ModifiedOn = board.ModifiedOn
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<BoardViewModel> result = await boardsController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<BoardViewModel>()
                .Id
                .Should()
                .Be(expectedId);
        }

        [Fact]
        public async Task IndexShouldReturnOkWhenSuccess()
        {
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>
                {
                    new Board {
                        Id = 1,
                        Title = "Board #1",
                    },
                    new Board {
                        Id = 2,
                        Title = "Board #2",
                    },
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult<IEnumerable<BoardViewModel>> result = await boardsController.Index();

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task IndexShouldReturnListOfBoardsWhenSuccess()
        {
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>
                {
                    new Board {
                        Id = 1,
                        Title = "Board #1",
                    },
                    new Board {
                        Id = 2,
                        Title = "Board #2",
                    },
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
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
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>
                {
                    new Board {
                        Id = 1,
                        Title = "Board #1",
                    },
                    new Board {
                        Id = 2,
                        Title = "Board #2",
                    },
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
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
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.GetAllUserBoards(It.IsAny<int>()))
                .ReturnsAsync(new List<Board>());
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
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

        [Fact]
        public async Task UpdateShouldReturnNoContentWhenSuccess()
        {
            var model = new PostBoardViewModel
            {
                Title = "Title #1"
            };
            int boardId = 1;
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.ExistsBoard(It.IsAny<int>()))
                .ReturnsAsync(true);
            fakeBoardRepository
                .Setup(repository => repository.GetBoardMember(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new BoardMember
                {
                    IsAdmin = true,
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnNotFoundWhenBoardNotExists()
        {
            var model = new PostBoardViewModel
            {
                Title = "Title #1"
            };
            int boardId = 1;
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.ExistsBoard(It.IsAny<int>()))
                .ReturnsAsync(false);
            fakeBoardRepository
                .Setup(repository => repository.GetBoardMember(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new BoardMember
                {
                    IsAdmin = true,
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnErrorViewModelWhenBoardNotExists()
        {
            var model = new PostBoardViewModel
            {
                Title = "Title #1"
            };
            int boardId = 1;
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.ExistsBoard(It.IsAny<int>()))
                .ReturnsAsync(false);
            fakeBoardRepository
                .Setup(repository => repository.GetBoardMember(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new BoardMember
                {
                    IsAdmin = true,
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task UpdateShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            var model = new PostBoardViewModel
            {
                Title = "Title #1"
            };
            int boardId = 1;
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.ExistsBoard(It.IsAny<int>()))
                .ReturnsAsync(true);
            fakeBoardRepository
                .Setup(repository => repository.GetBoardMember(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<BoardMember>(null));
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task UpdateShouldReturnForbidWhenUserIsNotBoardAdmin()
        {
            var model = new PostBoardViewModel
            {
                Title = "Title #1"
            };
            int boardId = 1;
            var fakeBoardRepository = new Mock<IBoardRepository>();
            fakeBoardRepository
                .Setup(repository => repository.ExistsBoard(It.IsAny<int>()))
                .ReturnsAsync(true);
            fakeBoardRepository
                .Setup(repository => repository.GetBoardMember(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new BoardMember
                {
                    IsAdmin = false,
                });
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext
            };

            ActionResult result = await boardsController.Update(model, boardId);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task ShowShouldReturnOkWhenSuccess()
        {
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardByIdWithListsTasksAndMembers(GetDefaultBoard(withId: boardId, withMemberId: 1));
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper("Url");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShowShouldReturnDetailedBoardViewModelWhenSuccess()
        {
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardByIdWithListsTasksAndMembers(GetDefaultBoard(withId: boardId, withMemberId: 1));
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper("Url");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeOfType<DetailedBoardViewModel>();
        }

        [Fact]
        public async Task ShowShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardByIdWithListsTasksAndMembers(null);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper("Url");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShowShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardByIdWithListsTasksAndMembers(null);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper("Url");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task ShowShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockGetBoardByIdWithListsTasksAndMembers(null);
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper("Url");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

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
        public async Task ShowShouldReturnForbidWhenUserIsNotMemberOfTheBoard()
        {
            int boardId = 1;
            Mock<IBoardRepository> fakeBoardRepository = IBoardRepositoryMock
                .Mock()
                .MockExistsBoard(exists: false)
                .MockGetBoardByIdWithListsTasksAndMembers(GetDefaultBoard(withId: boardId, withMemberId: 1));
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();

            ControllerContext fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "2");
            IUrlHelper fakeUrlHelper = GetFakeUrlHelper("Url");

            var boardsController = new BoardsController(
                fakeBoardRepository.Object,
                fakeDateTimeProvider.Object)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult<DetailedBoardViewModel> result = await boardsController.Show(boardId);

            result.Result.Should().BeOfType<ForbidResult>();
        }

        public Board GetDefaultBoard(int withId, int withMemberId)
        {
            var boardMember = new BoardMember
            {
                User = new User
                {
                    Id = withMemberId
                },
                IsAdmin = false
            };
            var task = new KanbanTask
            {
                Id = 1,
                Summary = "Task #1",
                Description = "Description",
                TagColor = "FFFFF",
                Assignments = new List<BoardMember> {
                    boardMember
                }
            };
            var list = new KanbanList
            {
                Id = 1,
                Title = "Todo",
                Tasks = new List<KanbanTask>
                {
                    task
                }
            };
            var board = new Board
            {
                Id = withId,
                Title = "Title",
                Members = new List<BoardMember> {
                    boardMember
                },
                Lists = new List<KanbanList> {
                    list
                }
            };
            return board;
        }
    }
}
