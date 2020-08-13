using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class PostTaskViewModel
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public string TagColor { get; set; }
        public IEnumerable<int> AssignedTo { get; set; }
        public int List { get; set; }
    }
}
