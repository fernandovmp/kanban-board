namespace KanbanBoard.WebApi.Repositories.QueryBuilder
{
    public interface IPatchQueryBuilder<TPatchParams>
    {
        (string query, TPatchParams queryParams) Build();
    }
}
