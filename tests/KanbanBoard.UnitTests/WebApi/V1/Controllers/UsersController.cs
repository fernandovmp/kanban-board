using System.Threading.Tasks;
using FluentAssertions;
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
    public class UsersControllerTests
    {

        [Fact]
        public async Task CreateShouldReturnCreatedAtActionWhenSuccess()
        {
            var model = new SignUpViewModel
            {
                Name = "default",
                Email = "email@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.Insert(It.IsAny<User>()))
                .ReturnsAsync((User user) => new User
                {
                    Id = 1,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password
                });
            fakeUserRepository
                .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                .Returns("HashedPassword");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateShouldReturnUserWithAnIdWhenSuccess()
        {
            int expectedId = 1;
            var model = new SignUpViewModel
            {
                Name = "default",
                Email = "email@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.Insert(It.IsAny<User>()))
                .ReturnsAsync((User user) => new User
                {
                    Id = expectedId,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password
                });
            fakeUserRepository
                .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                .Returns(Task.FromResult(false));
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            fakePasswordHasher
                .Setup(passwordHasher => passwordHasher.Hash(It.IsAny<string>()))
                .Returns("HashedPassword");
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result
                .Result
                .As<CreatedAtActionResult>()
                .Value
                .As<UserViewModel>()
                .Id
                .Should()
                .Be(expectedId);
        }

        [Fact]
        public async Task CreateShouldReturnConflictWhenEmailIsAlreadyInUse()
        {
            var model = new SignUpViewModel
            {
                Name = "default",
                Email = "email@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result
                .Result
                .Should()
                .BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task CreateShouldReturnErrorViewModelWhenEmailIsAlreadyInUse()
        {
            var model = new SignUpViewModel
            {
                Name = "default",
                Email = "email@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.ExistsUserWithEmail(It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Create(model);

            result
                .Result
                .As<ConflictObjectResult>()
                .Value
                .Should()
                .BeOfType<ErrorViewModel>();
        }

        [Fact]
        public async Task ShowShouldReturnOkWhenSuccess()
        {
            int userId = 1;
            User userInDatabase = new User
            {
                Id = 1,
                Email = "email@example.com",
                Name = "default",
                Password = "password"
            };
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.GetById(It.IsAny<int>()))
                .ReturnsAsync(userInDatabase);
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Show(userId);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShowShouldReturnUserViewModelWhenSuccess()
        {
            int userId = 1;
            User userInDatabase = new User
            {
                Id = 1,
                Email = "email@example.com",
                Name = "default",
                Password = "password"
            };
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.GetById(It.IsAny<int>()))
                .ReturnsAsync(userInDatabase);
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Show(userId);

            result
                .Result
                .As<OkObjectResult>()
                .Value
                .Should()
                .BeOfType<UserViewModel>();
        }

        [Fact]
        public async Task ShowShouldReturnNotFoundWhenUserDoesntExists()
        {
            int userId = 1;
            var fakeUserRepository = new Mock<IUserRepository>();
            fakeUserRepository
                .Setup(repository => repository.GetById(It.IsAny<int>()))
                .Returns(Task.FromResult<User>(null));
            var fakePasswordHasher = new Mock<IPasswordHasherService>();
            var usersController = new UsersController(
                fakePasswordHasher.Object,
                fakeUserRepository.Object);

            ActionResult<UserViewModel> result = await usersController.Show(userId);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
