using KanbanBoard.WebApi.Models;

namespace KanbanBoard.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
