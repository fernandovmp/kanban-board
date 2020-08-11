using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IBoardRepository
    {
        Task<IEnumerable<Board>> GetAllUserBoards(int userId);
        Task<Board> Insert(Board board);
        Task InsertBoardMember(BoardMember boardMember);
    }
}
