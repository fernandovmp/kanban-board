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

        public FakeBoardRepository()
        {
            var defaultDate = new DateTime(2020, 1, 1);
            var defaultUser = new User
            {
                Id = 1,
                Name = "Nero",
                Email = "email@example.com",
                Password = "SecretPassword"
            };
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
            task.Assignments.Add(member);
            list.Tasks.Add(task);
            list.Tasks.Add(task2);
            board.Lists.Add(list);
            board.Lists.Add(list2);
            board.Members.Add(member);
            _boards.Add(board);
        }

        private Task<T> Async<T>(T result) => Task.FromResult(result);

        public Task<bool> ExistsBoard(int boardId) => Async(_boards.Exists(board => board.Id == boardId));

        public Task<IEnumerable<Board>> GetAllUserBoards(int userId) =>
            Async(_boards.Where(board => board.Members.Any(member => member.User.Id == userId)));

        public Task<Board> GetBoardByIdWithListsTasksAndMembers(int boardId) =>
            Async(_boards.FirstOrDefault(board => board.Id == boardId));

        public Task<KanbanList> GetBoardList(int boardId, int listId) =>
            Async(_boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Lists
                .FirstOrDefault(list => list.Id == listId));

        public Task<BoardMember> GetBoardMember(int boardId, int userId) =>
            Async(_boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Members
                .FirstOrDefault(member => member.User.Id == userId));

        public Task<KanbanTask> GetBoardTask(int boardId, int taskId) =>
            Async(_boards
                .FirstOrDefault(board => board.Id == boardId)
                ?.Lists
                .SelectMany(list => list.Tasks)
                .FirstOrDefault(task => task.Id == taskId));

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

        public Task InsertBoardMember(BoardMember boardMember) => Async(new BoardMember
        {
            Board = boardMember.Board,
            CreatedOn = boardMember.CreatedOn,
            IsAdmin = boardMember.IsAdmin,
            ModifiedOn = boardMember.ModifiedOn,
            User = boardMember.User
        });

        public Task<KanbanList> InsertKanbanList(KanbanList list) => Async(new KanbanList
        {
            Id = GetMaxKanbanListId() + 1,
            Board = list.Board,
            CreatedOn = list.CreatedOn,
            ModifiedOn = list.ModifiedOn,
            Tasks = list.Tasks,
            Title = list.Title
        });

        private int GetMaxKanbanListId() => _boards.SelectMany(board => board.Lists).Max(list => list.Id);

        public Task<KanbanTask> InsertKanbanTask(KanbanTask task) => Async(new KanbanTask
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

        public Task Update(Board board) => Task.CompletedTask;
    }
}
