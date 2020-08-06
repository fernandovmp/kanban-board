using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Extensions;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{

    [ApiController]
    [Route("api/v1/users")]
    public class UsersController : ControllerBase
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
                return this.V1NotFound("User not found");
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
                return BadRequest(new ValidationErrorViewModel(
                    status: 400,
                    message: "Couldn't create the user",
                    errors: new List<ValidationError>() { new ValidationError("Email", "Email already in use") }
                    )
                );
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
                userId = userViewModel.Id
            };

            return CreatedAtAction(actionName: nameof(Show), routeValues, value: userViewModel);
        }
    }
}
