using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IBoardMemberRepository
    {
        Task<int> CountMembers(int boardId);
        Task<IEnumerable<BoardMember>> GetAllMembersOfTheBoard(int boardId);
        Task<BoardMember> GetByBoardIdAndUserId(int boardId, int userId);
        Task Insert(BoardMember boardMember);
        Task Remove(BoardMember boardMember);
    }
}
