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
    [Route("api/v{version:ApiVersion}/boards")]
    public class BoardsController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardMemberRepository _memberRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public BoardsController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider,
            IBoardMemberRepository memberRepository)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardViewModel>>> Index()
        {
            int userId = int.Parse(HttpContext.User.Identity.Name);

            IEnumerable<Board> boards = await _boardRepository.GetAllUserBoards(userId);
            IEnumerable<BoardViewModel> boardsViewModel = boards.Select(board => new BoardViewModel
            {
                Id = board.Id,
                Title = board.Title,
                CreatedOn = board.CreatedOn,
                ModifiedOn = board.ModifiedOn
            });

            return Ok(boardsViewModel);
        }

        [HttpGet("{boardId}")]
        public async Task<ActionResult<DetailedBoardViewModel>> Show(int boardId)
        {
            Board board = await _boardRepository.GetByIdWithListsTasksAndMembers(boardId);
            if (board is null)
            {
                return V1NotFound("Board not found");
            }
            int userId = int.Parse(HttpContext.User.Identity.Name);
            if (!board.Members.Exists(member => member.User.Id == userId))
            {
                return Forbid();
            }

            IEnumerable<BoardMemberViewModel> boardMembersViewModel = board.Members.Select(member => new BoardMemberViewModel
            {
                Id = member.User.Id,
                Name = member.User.Name,
                Email = member.User.Email,
                IsAdmin = member.IsAdmin
            });

            Func<BoardMember, string> getUserLink = member => Url
                .ActionLink(action: nameof(UsersController.Show),
                            controller: "Users",
                            values: new { version = "1", userId = member.User.Id });
            IEnumerable<BoardListViewModel> listsViewModel = board.Lists.Select(list => new BoardListViewModel
            {
                Id = list.Id,
                Title = list.Title,
                Tasks = list.Tasks.Select(task => new ListTaskViewModel
                {
                    Id = task.Id,
                    Description = task.Description ?? "",
                    Summary = task.Summary,
                    TagColor = task.TagColor,
                    AssignedTo = task.Assignments.Select(getUserLink)
                })
            });
            var boardViewModel = new DetailedBoardViewModel
            {
                Id = board.Id,
                Title = board.Title,
                Lists = listsViewModel,
                Members = boardMembersViewModel
            };
            return Ok(boardViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<BoardViewModel>> Create(PostBoardViewModel model)
        {
            DateTime createdDate = _dateTimeProvider.UtcNow();
            int userId = int.Parse(HttpContext.User.Identity.Name);
            var user = new User
            {
                Id = userId
            };
            var board = new Board
            {
                Title = model.Title,
                CreatedBy = user,
                CreatedOn = createdDate,
                ModifiedOn = createdDate
            };

            Board createdBoard = await _boardRepository.Insert(board);

            var boardAdmin = new BoardMember
            {
                Board = createdBoard,
                User = user,
                IsAdmin = true
            };

            await _memberRepository.Insert(boardAdmin);

            object routeValues = new
            {
                version = "1",
                boardId = createdBoard.Id
            };

            var boardViewModel = new BoardViewModel
            {
                Id = createdBoard.Id,
                Title = createdBoard.Title,
                CreatedOn = createdBoard.CreatedOn,
                ModifiedOn = createdBoard.ModifiedOn
            };

            return CreatedAtAction(actionName: nameof(Show), routeValues, value: boardViewModel);
        }

        [HttpPut("{boardId}")]
        public async Task<ActionResult> Update(PostBoardViewModel model, int boardId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);

            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);

            BoardMember member = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);

            if (member is null || !member.IsAdmin)
            {
                return Forbid();
            }

            Board board = new Board
            {
                Id = boardId,
                Title = model.Title,
                ModifiedOn = _dateTimeProvider.UtcNow()
            };

            await _boardRepository.Update(board);

            return NoContent();
        }

        [HttpDelete("{boardId}")]
        public async Task<ActionResult> Delete(int boardId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember member = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (member is null || !member.IsAdmin)
            {
                return Forbid();
            }

            await _boardRepository.Remove(boardId);
            return NoContent();
        }
    }
}
