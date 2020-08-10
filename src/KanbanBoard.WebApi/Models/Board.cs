using System;

namespace KanbanBoard.WebApi.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
