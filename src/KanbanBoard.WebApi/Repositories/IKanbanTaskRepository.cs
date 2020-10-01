using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IKanbanTaskRepository
    {
        Task<IEnumerable<KanbanTask>> GetAllTasksOfTheBoard(int boardId);
        Task<KanbanTask> GetByIdAndBoardId(int taskId, int boardId);
        Task<KanbanTask> Insert(KanbanTask task);
        Task Remove(KanbanTask task);
    }
}
