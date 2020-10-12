using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class ListTaskViewModel
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string TagColor { get; set; }
        public IEnumerable<string> AssignedTo { get; set; }
    }
}
