using System;
using System.Collections.Generic;

namespace KanbanBoard.WebApi.Models
{
    public class KanbanTask
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string TagColor { get; set; }
        public Board Board { get; set; }
        public KanbanList List { get; set; }
        public List<BoardMember> Assignments { get; set; } = new List<BoardMember>();
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
