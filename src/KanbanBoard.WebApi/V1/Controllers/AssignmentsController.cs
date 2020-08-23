using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{

    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:ApiVersion}/boards/{boardId}/tasks/{taskId}/assignments")]
    public class AssignmentsController : V1ControllerBase
    {
        private readonly IBoardRepository _boardRepository;

        public AssignmentsController(
            IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        [HttpPut("{memberId}")]
        public async Task<ActionResult> Create(int boardId, int taskId, int memberId)
        {
            bool boardExists = await _boardRepository.ExistsBoard(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember userMember = await _boardRepository.GetBoardMember(boardId, userId);
            if (userMember is null)
            {
                return Forbid();
            }
            KanbanTask task = await _boardRepository.GetBoardTask(boardId, taskId);
            if (task is null)
            {
                return V1NotFound("Task not found");
            }
            BoardMember member = await _boardRepository.GetBoardMember(boardId, memberId);
            if (member is null)
            {
                return V1NotFound("Member not found");
            }

            bool existsAssignment = await _boardRepository.ExistsAssignment(taskId, member);
            if (!existsAssignment)
            {
                await _boardRepository.CreateAssignment(taskId, member);
            }
            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<ActionResult> Delete(int boardId, int taskId, int memberId)
        {
            bool boardExists = await _boardRepository.ExistsBoard(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember userMember = await _boardRepository.GetBoardMember(boardId, userId);
            if (userMember is null)
            {
                return Forbid();
            }
            KanbanTask task = await _boardRepository.GetBoardTask(boardId, taskId);
            if (task is null)
            {
                return V1NotFound("Task not found");
            }

            var member = new BoardMember
            {
                User = new User
                {
                    Id = memberId
                },
                Board = new Board
                {
                    Id = boardId
                }
            };
            await _boardRepository.RemoveAssignment(taskId, member);

            return NoContent();
        }
    }
}
