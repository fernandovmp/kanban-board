using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Repositories.QueryBuilder;

namespace KanbanBoard.UnitTests.WebApi.Fakes.Repositories
{
    public class FakeKanbanTaskRepository : IKanbanTaskRepository
    {
        private readonly IReadOnlyCollection<Board> _boards;

        public FakeKanbanTaskRepository()
        {
            _boards = new FakeBoardRepository().Boards;
        }

        public Task<IEnumerable<KanbanTask>> GetAllTasksOfTheBoard(int boardId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Lists
                .SelectMany(list => list.Tasks)
        );

        public Task<KanbanTask> GetByIdAndBoardId(int taskId, int boardId) => Async(
            _boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Lists
                .SelectMany(list => list.Tasks)
                .FirstOrDefault(task => task.Id == taskId)
        );
        public Task<KanbanTask> Insert(KanbanTask task) => Async(new KanbanTask
        {
            Id = GetMaxKanbanTaskId() + 1,
            Assignments = task.Assignments,
            Board = task.Board,
            CreatedOn = task.CreatedOn,
            Description = task.Description,
            List = task.List,
            ModifiedOn = task.ModifiedOn,
            Summary = task.Summary,
            TagColor = task.TagColor
        });

        private int GetMaxKanbanTaskId() => _boards
            .SelectMany(board => board.Lists)
            .SelectMany(list => list.Tasks)
            .Max(task => task.Id);

        private Task<T> Async<T>(T result) => Task.FromResult(result);

        public Task Remove(KanbanTask task) => Task.CompletedTask;
        public Task Update(IPatchQueryBuilder<PatchTaskParams> patchTaskQueryBuilder) => Task.CompletedTask;
    }
}
