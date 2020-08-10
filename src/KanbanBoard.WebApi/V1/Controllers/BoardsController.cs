using System;
using System.Collections.Generic;
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
    }
}
