using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/boards/{boardId}/members")]
    public class BoardMembersController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardMemberRepository _memberRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public BoardMembersController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider,
            IBoardMemberRepository memberRepository)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardMemberViewModel>>> Index(int boardId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            IEnumerable<BoardMember> members = await _memberRepository.GetAllMembersOfTheBoard(boardId);

            int userId = int.Parse(HttpContext.User.Identity.Name);
            if (!members.Any(member => member.User.Id == userId))
            {
                return Forbid();
            }

            IEnumerable<BoardMemberViewModel> membersViewModel = members.Select(member => new BoardMemberViewModel
            {
                Id = member.User.Id,
                Name = member.User.Name,
                Email = member.User.Email,
                IsAdmin = member.IsAdmin
            });

            return Ok(membersViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(
            PostBoardMemberViewModel model,
            int boardId,
            [FromServices] IUserRepository userRepository)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }
            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember userMember = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (userMember is null || !userMember.IsAdmin)
            {
                return Forbid();
            }

            User newMemberUser = await userRepository.GetByEmailWithPassword(model.Email);
            if (newMemberUser is null)
            {
                return V1NotFound("User not found");
            }

            bool alreadyIsMember = await _memberRepository.GetByBoardIdAndUserId(boardId, newMemberUser.Id) is { };
            if (alreadyIsMember)
            {
                return NoContent();
            }

            DateTime createdDate = _dateTimeProvider.UtcNow();
            var member = new BoardMember
            {
                Board = new Board
                {
                    Id = boardId
                },
                User = new User
                {
                    Id = newMemberUser.Id,
                },
                IsAdmin = model.IsAdmin,
                CreatedOn = createdDate,
                ModifiedOn = createdDate
            };
            await _memberRepository.Insert(member);

            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<ActionResult> Delete(int boardId, int memberId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }
            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember userMember = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (userMember is null || !userMember.IsAdmin)
            {
                return Forbid();
            }
            int membersCount = await _memberRepository.CountMembers(boardId);
            if (membersCount <= 1)
            {
                return V1BadRequest("Board require at least one member");
            }

            var member = new BoardMember
            {
                Board = new Board
                {
                    Id = boardId
                },
                User = new User
                {
                    Id = memberId
                }
            };
            await _memberRepository.Remove(member);
            return NoContent();
        }
    }
}
