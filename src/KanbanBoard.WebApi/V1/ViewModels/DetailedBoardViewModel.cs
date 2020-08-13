using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class DetailedBoardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<BoardMemberViewModel> Members { get; set; }
        public IEnumerable<BoardListViewModel> Lists { get; set; }
    }
}
