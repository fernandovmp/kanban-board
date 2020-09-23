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
        private readonly IBoardMemberRepository _memberRepository;
        private readonly IKanbanTaskRepository _taskRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentsController(
            IBoardRepository boardRepository,
            IBoardMemberRepository memberRepository,
            IKanbanTaskRepository taskRepository,
            IAssignmentRepository assignmentRepository)
        {
            _boardRepository = boardRepository;
            _memberRepository = memberRepository;
            _taskRepository = taskRepository;
            _assignmentRepository = assignmentRepository;
        }

        [HttpPut("{memberId}")]
        public async Task<ActionResult> Create(int boardId, int taskId, int memberId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember userMember = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (userMember is null)
            {
                return Forbid();
            }
            KanbanTask task = await _taskRepository.GetByIdAndBoardId(taskId, boardId);
            if (task is null)
            {
                return V1NotFound("Task not found");
            }
            BoardMember member = await _memberRepository.GetByBoardIdAndUserId(boardId, memberId);
            if (member is null)
            {
                return V1NotFound("Member not found");
            }

            bool existsAssignment = await _assignmentRepository.ExistsAssignment(taskId, member);
            if (!existsAssignment)
            {
                await _assignmentRepository.Insert(taskId, member);
            }
            return NoContent();
        }

        [HttpDelete("{memberId}")]
        public async Task<ActionResult> Delete(int boardId, int taskId, int memberId)
        {
            bool boardExists = await _boardRepository.Exists(boardId);
            if (!boardExists)
            {
                return V1NotFound("Board not found");
            }

            int userId = int.Parse(HttpContext.User.Identity.Name);
            BoardMember userMember = await _memberRepository.GetByBoardIdAndUserId(boardId, userId);
            if (userMember is null)
            {
                return Forbid();
            }
            KanbanTask task = await _taskRepository.GetByIdAndBoardId(boardId, taskId);
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
            await _assignmentRepository.Remove(taskId, member);

            return NoContent();
        }
    }
}
