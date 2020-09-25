using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;

namespace KanbanBoard.UnitTests.WebApi.Fakes.Repositories
{
    public class FakeKanbanListRepository : IKanbanListRepository
    {
        private readonly IReadOnlyCollection<Board> _boards;

        public FakeKanbanListRepository()
        {
            _boards = new FakeBoardRepository().Boards;
        }

        public Task<IEnumerable<KanbanList>> GetAllListsOfTheBoard(int boardId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Lists
                .AsEnumerable()
        );

        public Task<KanbanList> GetByIdAndBoardIdWithTasks(int listId, int boardId) => GetByIdAndBoardId(listId, boardId);

        public Task<KanbanList> GetByIdAndBoardId(int listId, int boardId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Lists
                .FirstOrDefault(list => list.Id == listId)
        );

        public Task<KanbanList> Insert(KanbanList list) => Async(new KanbanList
        {
            Id = GetMaxKanbanListId() + 1,
            Board = list.Board,
            CreatedOn = list.CreatedOn,
            ModifiedOn = list.ModifiedOn,
            Tasks = list.Tasks,
            Title = list.Title
        });

        private Task<T> Async<T>(T result) => Task.FromResult(result);

        private int GetMaxKanbanListId() => _boards.SelectMany(board => board.Lists).Max(list => list.Id);

        public Task Remove(KanbanList list) => Task.CompletedTask;
        public Task Update(KanbanList list) => Task.CompletedTask;
    }
}
