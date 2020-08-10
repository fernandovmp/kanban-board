using System;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class BoardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
