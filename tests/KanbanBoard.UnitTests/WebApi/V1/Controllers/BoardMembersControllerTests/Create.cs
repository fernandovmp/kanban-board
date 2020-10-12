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
    public class CreateTests : BoardMembersControllerTestsBase
    {
        private readonly IUserRepository _fakeUserRepository;
        private readonly PostBoardMemberViewModel _validViewModel;
        private const int ExistentId = 1;
        private const int NonExistentId = 10;

        public CreateTests() : base()
        {
            _fakeUserRepository = new FakeUserRepository();
            _validViewModel = new PostBoardMemberViewModel
            {
                Email = "default@example.com",
                IsAdmin = false,
            };
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenSuccess()
        {
            int boardId = ExistentId;
            PostBoardMemberViewModel model = _validViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenTheNewMemberIsAlreadyMemberOfTheBoard()
        {
            int boardId = ExistentId;
            PostBoardMemberViewModel model = new PostBoardMemberViewModel
            {
                Email = "email@example.com",
                IsAdmin = false,
            };
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenBoardNotExists()
        {
            int boardId = NonExistentId;
            PostBoardMemberViewModel model = _validViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenBoardNotExists()
        {
            int boardId = NonExistentId;
            PostBoardMemberViewModel model = _validViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenBoardNotExists()
        {
            int boardId = NonExistentId;
            PostBoardMemberViewModel model = _validViewModel;
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result
                .As<NotFoundObjectResult>()
                .Value
                .As<ErrorViewModel>()
                .Status
                .Should()
                .Be(404);
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenUserNotExists()
        {
            int boardId = ExistentId;
            PostBoardMemberViewModel model = new PostBoardMemberViewModel
            {
                Email = "random@email.com",
                IsAdmin = false,
            };
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenUserNotExists()
        {
            int boardId = ExistentId;
            PostBoardMemberViewModel model = new PostBoardMemberViewModel
            {
                Email = "random@email.com",
                IsAdmin = false,
            };
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWithStatus404WhenUserNotExists()
        {
            int boardId = ExistentId;
            PostBoardMemberViewModel model = new PostBoardMemberViewModel
            {
                Email = "random@email.com",
                IsAdmin = false,
            };
            var fakeDateTimeProvider = new Mock<IDateTimeProvider>();
            var boardMembersController = new BoardMembersController(
                fakeBoardRepository,
                fakeDateTimeProvider.Object,
                fakeMemberRepository)
            {
                ControllerContext = fakeControllerContext,
                Url = fakeUrlHelper
            };

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

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
            int boardId = ExistentId;
            PostBoardMemberViewModel model = _validViewModel;
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

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task ShouldReturnForbidWhenUserIsNotAdminOfTheBoard()
        {
            int boardId = ExistentId;
            PostBoardMemberViewModel model = _validViewModel;
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

            ActionResult result = await boardMembersController.Create(model, boardId, _fakeUserRepository);

            result.Should().BeOfType<ForbidResult>();
        }
    }
}
