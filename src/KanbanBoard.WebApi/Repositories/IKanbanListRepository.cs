using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IKanbanListRepository
    {
        Task<KanbanList> GetByIdAndBoardId(int listId, int boardId);
        Task<KanbanList> Insert(KanbanList list);
        Task Remove(KanbanList list);
        Task Update(KanbanList list);
    }
}
