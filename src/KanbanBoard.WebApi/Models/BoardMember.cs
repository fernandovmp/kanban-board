using System;

namespace KanbanBoard.WebApi.Models
{
    public class BoardMember
    {
        public Board Board { get; set; }
        public User User { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
