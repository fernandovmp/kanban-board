namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class PatchTaskViewModel
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string TagColor { get; set; }
        public int List { get; set; }
    }
}
