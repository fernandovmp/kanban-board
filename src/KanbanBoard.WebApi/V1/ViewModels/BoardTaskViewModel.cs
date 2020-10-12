using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class BoardTaskViewModel
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string TagColor { get; set; }
        public int List { get; set; }
        public IEnumerable<UserViewModel> Assignments { get; set; }
    }
}
