using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IBoardRepository
    {
        Task<int> CountBoardMembers(int boardId);
        Task<bool> ExistsBoard(int boardId);
        Task<IEnumerable<BoardMember>> GetAllBoardMembers(int boardId);
        Task<IEnumerable<Board>> GetAllUserBoards(int userId);
        Task<Board> GetBoardByIdWithListsTasksAndMembers(int boardId);
        Task<KanbanList> GetBoardList(int boardId, int listId);
        Task<BoardMember> GetBoardMember(int boardId, int userId);
        Task<KanbanTask> GetBoardTask(int boardId, int taskId);
        Task<Board> Insert(Board board);
        Task InsertBoardMember(BoardMember boardMember);
        Task<KanbanList> InsertKanbanList(KanbanList list);
        Task<KanbanTask> InsertKanbanTask(KanbanTask task);
        Task RemoveBoardMember(BoardMember boardMember);
        Task Update(Board board);
        Task UpdateKanbanList(KanbanList list);
    }
}
