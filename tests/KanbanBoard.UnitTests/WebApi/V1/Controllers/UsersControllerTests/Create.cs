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
    public class CreateTests
    {
        private readonly IUserRepository _fakeUserRepository;
        private readonly SignUpViewModel _validSignUpViewModel;

        public CreateTests()
        {
            _fakeUserRepository = new FakeUserRepository();
            _validSignUpViewModel = new SignUpViewModel
            {
                Name = "Emilia",
                Email = "emilia@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
        }

        [Fact]
        public async Task ShouldReturnCreatedAtActionWhenSuccess()
        {
            SignUpViewModel model = _validSignUpViewModel;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                .Returns("HashedPassword");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task ShouldReturnUserWithAnIdWhenSuccess()
        {
            SignUpViewModel model = _validSignUpViewModel;
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                .Returns("HashedPassword");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<UserViewModel>()
                .Id
                .Should()
                .NotBe(0);
        }

        [Fact]
        public async Task ShouldReturnConflictWhenEmailIsAlreadyInUse()
        {
            SignUpViewModel model = new SignUpViewModel
            {
                Name = "default",
                Email = "email@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                .Returns("HashedPassword");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result
                .Result
                .Should()
                .BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnErrorViewModelWhenEmailIsAlreadyInUse()
        {
            SignUpViewModel model = new SignUpViewModel
            {
                Name = "default",
                Email = "email@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                .Returns("HashedPassword");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                _fakeUserRepository);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result
                .Result
                .As<ConflictObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }
    }
}
