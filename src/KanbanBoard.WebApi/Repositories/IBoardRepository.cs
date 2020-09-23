using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IBoardRepository
    {
        Task<bool> Exists(int boardId);
        Task<IEnumerable<Board>> GetAllUserBoards(int userId);
        Task<Board> GetByIdWithListsTasksAndMembers(int boardId);
        Task<Board> Insert(Board board);
        Task Remove(int boardId);
        Task Update(Board board);
    }
}
