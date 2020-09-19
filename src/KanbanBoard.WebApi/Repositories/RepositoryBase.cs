using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly IDbConnectionFactory connectionFactory;

        public RepositoryBase(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
    }
}
