using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class BoardListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<ListTaskViewModel> Tasks { get; set; }
    }
}
