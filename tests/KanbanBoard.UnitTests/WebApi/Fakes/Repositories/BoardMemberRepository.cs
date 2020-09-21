using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;

namespace KanbanBoard.UnitTests.WebApi.Fakes.Repositories
{
    public class FakeBoardMemberRepository : IBoardMemberRepository
    {
        private readonly IReadOnlyCollection<Board> _boards;

        public FakeBoardMemberRepository()
        {
            _boards = new FakeBoardRepository().Boards;
        }

        public Task<int> CountMembers(int boardId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Members
                .Count ?? 0
        );

        public Task<IEnumerable<BoardMember>> GetAllMembersOfTheBoard(int boardId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Members
                .AsEnumerable()
        );

        public Task<BoardMember> GetByBoardIdAndUserId(int boardId, int userId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Members
                .FirstOrDefault(member => member.User.Id == userId)
        );

        public Task Insert(BoardMember boardMember) => Task.CompletedTask;

        private Task<T> Async<T>(T result) => Task.FromResult(result);

        public Task Remove(BoardMember boardMember) => Task.CompletedTask;
    }
}
