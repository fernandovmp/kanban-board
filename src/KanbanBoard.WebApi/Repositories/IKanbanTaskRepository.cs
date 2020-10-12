using System.Collections.Generic;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories.QueryBuilder;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IKanbanTaskRepository
    {
        Task<IEnumerable<KanbanTask>> GetAllTasksOfTheBoard(int boardId);
        Task<KanbanTask> GetByIdAndBoardId(int taskId, int boardId);
        Task<KanbanTask> Insert(KanbanTask task);
        Task Update(IPatchQueryBuilder<PatchTaskParams> patchTaskQueryBuilder);
        Task Remove(KanbanTask task);
    }
}
