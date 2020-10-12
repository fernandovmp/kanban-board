using System.Data;

namespace KanbanBoard.WebApi.Services
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
