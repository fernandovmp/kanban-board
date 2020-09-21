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
        private readonly IKanbanTaskRepository _taskRepository;
        private readonly IKanbanListRepository _listRepository;
        private readonly IBoardMemberRepository _memberRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public TasksController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider,
            IKanbanTaskRepository taskRepository,
            IKanbanListRepository listRepository,
            IBoardMemberRepository memberRepository)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
            _taskRepository = taskRepository;
            _listRepository = listRepository;
            _memberRepository = memberRepository;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<BoardTaskViewModel>> Show(int boardId, int taskId)
        {
            bool boardExists = await _boardRepository.ExistsBoard(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember member = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (member is null)
            {
                return Forbid();
            }

            KanbanTask task = await _taskRepository.GetByIdAndBoardId(taskId, boardId);
            if (task is null)
            {
                return V1NotFound("Task not found");
            }

            var taskViewModel = new BoardTaskViewModel
            {
                Id = task.Id,
                Summary = task.Summary,
                Description = task.Description,
                TagColor = task.TagColor,
                List = task.List.Id,
                Assignments = task.Assignments.Select(member => new UserViewModel
                {
                    Id = member.User.Id,
                    Name = member.User.Name,
                    Email = member.User.Email
                })
            };

            return Ok(taskViewModel);
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
            BoardMember boardMember = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (boardMember is null)
            {
                return Forbid();
            }

            KanbanList list = await _listRepository.GetByIdAndBoardId(boardId, model.List);
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
            KanbanTask createdTask = await _taskRepository.Insert(task);

            object routeValues = new
            {
                version = "1",
                boardId,
                taskId = createdTask.Id
            };
            object listLinkValues = new
            {
                version = "1",
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

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> Delete(int boardId, int taskId)
        {
            bool boardExists = await _boardRepository.ExistsBoard(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember member = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (member is null)
            {
                return Forbid();
            }

            KanbanTask task = await _taskRepository.GetByIdAndBoardId(boardId, taskId);
            if (task is null)
            {
                return V1NotFound("Task not found");
            }

            await _taskRepository.Remove(task);
            return NoContent();
        }
    }
}
