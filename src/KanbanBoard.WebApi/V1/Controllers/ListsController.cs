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
    [Route("api/v1/boards/{boardId}/lists")]
    public class ListsController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ListsController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet("{listId}")]
        public Task<ActionResult> Show(int boardId, int listId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<KanbanListViewModel>> Create(PostListViewModel model, int boardId)
        {
            bool boardExists = await _boardRepository.ExistsBoard(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember boardMember = await _boardRepository.GetBoardMember(boardId, userId);
            if (boardMember is null)
            {
                return Forbid();
            }

            var board = new Board
            {
                Id = boardId
            };
            DateTime createdDate = _dateTimeProvider.UtcNow();
            var list = new KanbanList
            {
                Title = model.Title,
                Board = board,
                CreatedOn = createdDate,
                ModifiedOn = createdDate
            };
            KanbanList createdList = await _boardRepository.InsertKanbanList(list);

            object routeValues = new
            {
                boardId,
                listId = createdList.Id
            };
            var kanbanListViewModel = new KanbanListViewModel
            {
                Id = createdList.Id,
                Title = createdList.Title,
                Tasks = new List<string>(),
                CreatedOn = createdList.CreatedOn,
                ModifiedOn = createdList.ModifiedOn
            };

            return CreatedAtAction(actionName: nameof(Show), routeValues, value: kanbanListViewModel);
        }
    }
}