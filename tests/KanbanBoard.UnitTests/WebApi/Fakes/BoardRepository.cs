using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;

namespace KanbanBoard.UnitTests.WebApi.Fakes
{
    public class FakeBoardRepository : IBoardRepository
    {
        private readonly List<Board> _boards;
        internal IReadOnlyCollection<Board> Boards => _boards;

        public FakeBoardRepository()
        {
            var defaultDate = new DateTime(2020, 1, 1);
            FakeUserRepository userRepository = new FakeUserRepository();
            User defaultUser = userRepository.GetById(1).Result;
            _boards = new List<Board>();
            var board = new Board
            {
                Id = 1,
                Title = "Project Board",
                CreatedBy = defaultUser,
                Lists = new List<KanbanList>(),
                CreatedOn = defaultDate,
                ModifiedOn = defaultDate
            };
            var list = new KanbanList
            {
                Board = board,
                Id = 1,
                CreatedOn = defaultDate,
                ModifiedOn = defaultDate,
                Tasks = new List<KanbanTask>(),
                Title = "Todo"
            };
            var list2 = new KanbanList
            {
                Board = board,
                Id = 2,
                CreatedOn = defaultDate,
                ModifiedOn = defaultDate,
                Tasks = new List<KanbanTask>(),
                Title = "Doing"
            };
            var task = new KanbanTask
            {
                Board = board,
                Id = 1,
                List = list,
                CreatedOn = defaultDate,
                Description = "Task description",
                ModifiedOn = defaultDate,
                Summary = "Task #1",
                TagColor = "FFFFFF",
                Assignments = new List<BoardMember>(),
            };
            var task2 = new KanbanTask
            {
                Board = board,
                Id = 2,
                List = list,
            };
            var member = new BoardMember
            {
                Board = board,
                User = defaultUser,
                IsAdmin = true,
                CreatedOn = defaultDate,
                ModifiedOn = defaultDate
            };
            User user = userRepository.GetById(2).Result;
            var member2 = new BoardMember
            {
                Board = board,
                User = user,
                IsAdmin = false,
                CreatedOn = defaultDate,
                ModifiedOn = defaultDate
            };
            task.Assignments.Add(member);
            list.Tasks.Add(task);
            list.Tasks.Add(task2);
            board.Lists.Add(list);
            board.Lists.Add(list2);
            board.Members.Add(member);
            board.Members.Add(member2);
            _boards.Add(board);
        }

        public Task<bool> Exists(int boardId) => Async(_boards.Exists(board => board.Id == boardId));

        public Task<IEnumerable<Board>> GetAllUserBoards(int userId) =>
            Async(_boards.Where(board => board.Members.Any(member => member.User.Id == userId)));

        public Task<Board> GetByIdWithListsTasksAndMembers(int boardId) =>
            Async(_boards.FirstOrDefault(board => board.Id == boardId));

        public Task<Board> Insert(Board board) => Async(new Board
        {
            Id = _boards.Max(board => board.Id) + 1,
            CreatedBy = board.CreatedBy,
            CreatedOn = board.CreatedOn,
            Lists = board.Lists,
            Members = board.Members,
            ModifiedOn = board.ModifiedOn,
            Title = board.Title
        });

        public Task Update(Board board) => Task.CompletedTask;

        public Task Remove(int boardId) => Task.CompletedTask;

        private Task<T> Async<T>(T result) => Task.FromResult(result);
    }
}
