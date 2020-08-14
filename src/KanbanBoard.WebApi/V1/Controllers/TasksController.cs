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
    [Route("api/v{version:ApiVersion}/boards/{boardId}/tasks")]
    public class TasksController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TasksController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet("{taskId}")]
        public Task<ActionResult> Show(int boardId, int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<KanbanTaskViewModel>> Create(PostTaskViewModel model, int boardId)
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

            KanbanList list = await _boardRepository.GetBoardList(boardId, model.List);
            if (list is null)
            {
                return Forbid();
            }

            var board = new Board
            {
                Id = boardId
            };
            DateTime createdDate = _dateTimeProvider.UtcNow();
            var task = new KanbanTask
            {
                Summary = model.Summary,
                Description = model.Description,
                TagColor = model.TagColor,
                Board = board,
                List = list,
                CreatedOn = createdDate,
                ModifiedOn = createdDate
            };
            KanbanTask createdTask = await _boardRepository.InsertKanbanTask(task);

            object routeValues = new
            {
                version = "1",
                boardId,
                taskId = createdTask.Id
            };
            object listLinkValues = new
            {
                boardId,
                listId = createdTask.List.Id
            };
            string listLink = Url
                .ActionLink(action: nameof(ListsController.Show), controller: "Lists", listLinkValues);
            var kanbanListViewModel = new KanbanTaskViewModel
            {
                Id = createdTask.Id,
                Summary = createdTask.Summary,
                Description = createdTask.Description,
                TagColor = createdTask.TagColor,
                AssignedTo = new List<string>(),
                List = listLink
            };

            return CreatedAtAction(actionName: nameof(Show), routeValues, value: kanbanListViewModel);
        }
    }
}
