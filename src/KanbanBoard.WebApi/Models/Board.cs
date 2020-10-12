using System;
using System.Collections.Generic;

namespace KanbanBoard.WebApi.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<BoardMember> Members { get; set; } = new List<BoardMember>();
        public List<KanbanList> Lists { get; set; } = new List<KanbanList>();
        public User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
