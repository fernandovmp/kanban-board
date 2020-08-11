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
    [Route("api/v1/boards")]
    public class BoardsController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public BoardsController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
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
        public Task<ActionResult> Show(int boardId)
        {
            throw new NotImplementedException();
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

            await _boardRepository.InsertBoardMember(boardAdmin);

            object routeValues = new
            {
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
            bool boardExists = await _boardRepository.ExistsBoard(boardId);

            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);

            BoardMember member = await _boardRepository.GetBoardMember(boardId, userId);

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
    }
}
