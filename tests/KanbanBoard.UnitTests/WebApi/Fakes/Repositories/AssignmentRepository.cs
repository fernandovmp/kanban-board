using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;

namespace KanbanBoard.UnitTests.WebApi.Fakes.Repositories
{
    public class FakeAssignmentRepository : IAssignmentRepository
    {
        private readonly IReadOnlyCollection<Board> _boards;

        public FakeAssignmentRepository()
        {
            _boards = new FakeBoardRepository().Boards;
        }

        public Task<bool> ExistsAssignment(int taskId, BoardMember member) => Async(_boards
            .FirstOrDefault(board => board.Id == member.Board.Id)
            ?.Lists
            .SelectMany(list => list.Tasks)
            .FirstOrDefault(task => task.Id == taskId)
            ?.Assignments
            .Any(assignment => assignment.User.Id == member.User.Id) ?? false
        );

        private Task<T> Async<T>(T result) => Task.FromResult(result);

        public Task Insert(int taskId, BoardMember member) => Task.CompletedTask;
        public Task Remove(int taskId, BoardMember boardMember) => Task.CompletedTask;
        public Task Insert(int taskId, IEnumerable<BoardMember> members) => Task.CompletedTask;
    }
}
