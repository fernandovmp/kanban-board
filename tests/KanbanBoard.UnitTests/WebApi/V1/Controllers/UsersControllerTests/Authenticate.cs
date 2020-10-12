using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.UsersControllerTests
{
    [Trait("Category", "UsersController")]
    public class AuthenticateTests
    {
        private readonly IUserRepository _fakeUserRepository;
        private readonly LogInViewModel _validLogInUpViewModel;

        public AuthenticateTests()
        {
            _fakeUserRepository = new FakeUserRepository();
            _validLogInUpViewModel = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "password"
            };
        }

        [Fact]
        public async Task AuthenticateShouldReturnOkWhenSuccess()
        {
            LogInViewModel model = _validLogInUpViewModel;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AuthenticateShouldReturnLoginResponseViewModelWhenSuccess()
        {
            LogInViewModel model = _validLogInUpViewModel;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeOfType<LogInResponseViewModel>();
        }

        [Fact]
        public async Task AuthenticateShouldReturnUserWhenSuccess()
        {
            LogInViewModel model = _validLogInUpViewModel;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<LogInResponseViewModel>()
                .User
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task AuthenticateShouldReturnTokenWhenSuccess()
        {
            LogInViewModel model = _validLogInUpViewModel;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var fakeTokenService = new Mock<ITokenService>();
            fakeTokenService
                .Setup(tokenService => tokenService.GenerateToken(It.IsAny<User>()))
                .Returns("Token");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .As<LogInResponseViewModel>()
                .Token
                .Should()
                .NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task AuthenticateShouldReturnNotFoundWhenUserNotExists()
        {
            var model = new LogInViewModel
            {
                Email = "myemail@example.com",
                Password = "password"
            };
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .Should()
                .BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task AuthenticateShouldReturnErrorViewModelWhenUserNotExists()
        {
            var model = new LogInViewModel
            {
                Email = "myemail@example.com",
                Password = "password"
            };
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .As<NotFoundObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task AuthenticateShouldReturnBadRequestWhenCredentialsAreInvalid()
        {
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "wrongpassword"
            };
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AuthenticateShouldReturnErrorViewModelWhenCredentialsAreInvalid()
        {
            var model = new LogInViewModel
            {
                Email = "email@example.com",
                Password = "password"
            };
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);
            var fakeTokenService = new Mock<ITokenService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<LogInResponseViewModel> result = await usersController.Authenticate(model, fakeTokenService.Object);

            result
                .Result
                .As<BadRequestObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }
    }
}
