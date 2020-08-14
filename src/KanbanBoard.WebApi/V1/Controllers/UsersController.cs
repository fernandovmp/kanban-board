using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/users")]
    public class UsersController : V1ControllerBase
    {
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IUserRepository _userRepository;

        public UsersController(IPasswordHasherService passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserViewModel>> Show(int userId)
        {
            User user = await _userRepository.GetById(userId);

            if (user is null)
            {
                return V1NotFound("User not found");
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            return Ok(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> Create(SignUpViewModel model)
        {
            string hashedPassword = _passwordHasher.Hash(model.Password);

            bool isEmailAlreadyInUse = await _userRepository.ExistsUserWithEmail(model.Email);

            if (isEmailAlreadyInUse)
            {
                return V1Conflict(message: "Email already in use");
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashedPassword
            };

            User createdUser = await _userRepository.Insert(user);

            var userViewModel = new UserViewModel
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
            };

            object routeValues = new
            {
                version = "1",
                userId = userViewModel.Id
            };

            return CreatedAtAction(actionName: nameof(Show), routeValues, value: userViewModel);
        }

        [HttpPost("/api/v{version:ApiVersion}/login")]
        public async Task<ActionResult<LogInResponseViewModel>> Authenticate(
            LogInViewModel model,
            [FromServices] ITokenService tokenService
        )
        {
            User user = await _userRepository.GetByEmailWithPassword(model.Email);

            if (user is null)
            {
                return V1NotFound(message: "User not found");
            }

            bool correctPassword = _passwordHasher.VerifyPassword(user.Password, model.Password);

            if (!correctPassword)
            {
                return V1BadRequest(message: "Invalid credentials");
            }

            string jwtToken = tokenService.GenerateToken(user);

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            var responseViewModel = new LogInResponseViewModel
            {
                Token = jwtToken,
                User = userViewModel
            };

            return Ok(responseViewModel);
        }
    }
}
