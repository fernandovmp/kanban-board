using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IKanbanListRepository
    {
        Task<IEnumerable<KanbanList>> GetAllListsOfTheBoard(int boardId);
        Task<KanbanList> GetByIdAndBoardId(int listId, int boardId);
        Task<KanbanList> GetByIdAndBoardIdWithTasks(int listId, int boardId);
        Task<KanbanList> Insert(KanbanList list);
        Task Remove(KanbanList list);
        Task Update(KanbanList list);
    }
}
