using System;
using System.Collections.Generic;

namespace KanbanBoard.WebApi.Models
{
    public class KanbanList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<KanbanTask> Tasks { get; set; } = new List<KanbanTask>();
        public Board Board { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
