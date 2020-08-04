using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsUserWithEmail(string email);
        Task<User> Insert(User user);
    }
}
