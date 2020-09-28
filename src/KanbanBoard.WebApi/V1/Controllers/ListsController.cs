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
    [Route("api/v{version:ApiVersion}/boards/{boardId}/lists")]
    public class ListsController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardMemberRepository _memberRepository;
        private readonly IKanbanListRepository _listRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ListsController(
            IBoardRepository boardRepository,
            IDateTimeProvider dateTimeProvider,
            IBoardMemberRepository memberRepository,
            IKanbanListRepository listRepository)
        {
            _boardRepository = boardRepository;
            _dateTimeProvider = dateTimeProvider;
            _memberRepository = memberRepository;
            _listRepository = listRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KanbanListViewModel>>> Index(int boardId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
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

            IEnumerable<KanbanList> boardLists = await _listRepository.GetAllListsOfTheBoard(boardId);
            IEnumerable<KanbanListViewModel> viewModel = boardLists.Select(list => new KanbanListViewModel
            {
                Id = list.Id,
                Title = list.Title,
                Tasks = list.Tasks.Select(task => ResolveTaskUrl(task, boardId)),
                CreatedOn = list.CreatedOn,
                ModifiedOn = list.ModifiedOn
            });

            return Ok(viewModel);
        }

        private string ResolveTaskUrl(KanbanTask task, int boardId) =>
            Url.ActionLink(nameof(TasksController.Show), "Tasks", new
            {
                version = "1",
                boardId,
                taskId = task.Id
            });

        [HttpGet("{listId}")]
        public async Task<ActionResult<BoardListViewModel>> Show(int boardId, int listId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
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

            KanbanList list = await _listRepository.GetByIdAndBoardIdWithTasks(listId, boardId);
            if (list is null)
            {
                return V1NotFound("List not found");
            }

            BoardListViewModel viewModel = new BoardListViewModel
            {
                Id = list.Id,
                Title = list.Title,
                Tasks = list.Tasks.Select(task => new ListTaskViewModel
                {
                    Id = task.Id,
                    Summary = task.Summary,
                    Description = task.Description,
                    TagColor = task.TagColor,
                    AssignedTo = task.Assignments.Select(ResolveMemberUrl).ToList()
                })
            };
            return Ok(viewModel);
        }

        private string ResolveMemberUrl(BoardMember member) =>
            Url.ActionLink(nameof(UsersController.Show), "Users", new
            {
                version = "1",
                userId = member.User.Id
            });

        [HttpPost]
        public async Task<ActionResult<KanbanListViewModel>> Create(PostListViewModel model, int boardId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
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
            KanbanList createdList = await _listRepository.Insert(list);

            object routeValues = new
            {
                version = "1",
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

        [HttpPut("{listId}")]
        public async Task<ActionResult> Update(PostListViewModel model, int boardId, int listId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
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

            KanbanList storedList = await _listRepository.GetByIdAndBoardId(listId, boardId);
            if (storedList is null)
            {
                return V1NotFound("List not found");
            }

            if (storedList.Title != model.Title)
            {
                DateTime modifiedDate = _dateTimeProvider.UtcNow();
                var kanbanList = new KanbanList
                {
                    Id = listId,
                    Title = model.Title,
                    ModifiedOn = modifiedDate,
                };
                await _listRepository.Update(kanbanList);
            }

            return NoContent();
        }

        [HttpDelete("{listId}")]
        public async Task<ActionResult> Delete(int boardId, int listId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
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

            KanbanList list = new KanbanList
            {
                Board = new Board
                {
                    Id = boardId
                },
                Id = listId
            };
            await _listRepository.Remove(list);
            return NoContent();
        }
    }
}
