namespace KanbanBoard.WebApi.Repositories.QueryBuilder
{
    public class PatchTaskParams
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string TagColor { get; set; }
        public int List { get; set; }
    }
}
