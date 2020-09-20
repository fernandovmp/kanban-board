using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IAssignmentRepository
    {
        Task<bool> ExistsAssignment(int taskId, BoardMember member);
        Task Insert(int taskId, BoardMember member);
        Task Remove(int taskId, BoardMember boardMember);
    }
}
