using System;
using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class KanbanListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Tasks { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
