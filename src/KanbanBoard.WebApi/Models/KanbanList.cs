using System;

namespace KanbanBoard.WebApi.Models
{
    public class KanbanList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Board Board { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
