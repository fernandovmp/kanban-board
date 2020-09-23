using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private const string ExistsQuery = @"select 1 from boards where id = @Id;";
        private const string DeleteBoardQuery = @"delete from listTasks where listId in (select id from lists where boardId = @BoardId);
            delete from assignments where boardId = @BoardId;
            delete from boardMembers where boardId = @BoardId;
            delete from tasks where boardId = @BoardId;
            delete from lists where boardId = @BoardId;
            delete from boards where id = @BoardId;";
        private const string GetAllUserBoardsQuery = @"select boards.id, boards.title, boards.createdOn, boards.modifiedOn from boards
            inner join boardmembers on boardmembers.boardId = boards.id
            where boardmembers.userId = @UserId;";
        private const string GetByIdWithListsTasksAndMembersQuery = @"select boards.title, users.id, users.name, users.email, boardMembers.isAdmin,
            lists.id, lists.title, tasks.id, tasks.summary, tasks.tagColor, assignments.userId from boards
            left join boardMembers on boardMembers.boardId = boards.Id
            left join users on users.id = boardMembers.userId
            left join lists on lists.boardId = boards.id
            left join listTasks on listTasks.listId = lists.id
            left join tasks on listTasks.taskId = tasks.id
            left join assignments on assignments.taskId = tasks.id
            where boards.id = @BoardId;";
        private const string InsertQuery = @"insert into boards (title, createdOn, modifiedOn, createdBy)
            values (@Title, @CreatedOn, @ModifiedOn, @CreatedBy) returning id;";
        private const string UpdateQuery = @"update boards set title = @Title, modifiedOn = @ModifiedOn where id = @Id;";
        private readonly IDbConnectionFactory _connectionFactory;

        public BoardRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> Exists(int boardId)
        {
            object queryParams = new
            {
                Id = boardId
            };
            using IDbConnection connection = _connectionFactory.CreateConnection();
            bool exists = await connection.ExecuteScalarAsync<bool>(ExistsQuery, queryParams);
            return exists;
        }

        public async Task<IEnumerable<Board>> GetAllUserBoards(int userId)
        {
            object queryParams = new
            {
                UserId = userId
            };
            using IDbConnection connection = _connectionFactory.CreateConnection();
            IEnumerable<Board> boards = await connection.QueryAsync<Board>(GetAllUserBoardsQuery, queryParams);
            return boards;
        }

        public async Task<Board> GetByIdWithListsTasksAndMembers(int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId
            };

            Board boardCache = null;
            Dictionary<int, KanbanList> listCache = new Dictionary<int, KanbanList>();
            Dictionary<int, KanbanTask> taskCache = new Dictionary<int, KanbanTask>();

            using IDbConnection connection = _connectionFactory.CreateConnection();

            IEnumerable<Board> boards = await connection
                .QueryAsync<Board, User, BoardMember, KanbanList, KanbanTask, int?, Board>(
                    GetByIdWithListsTasksAndMembersQuery,
                    map: (board, user, member, list, task, userId) =>
                    {
                        if (boardCache is null)
                        {
                            boardCache = board;
                        }
                        if (member is object && !boardCache.Members.Any(_member => _member.User.Id == user.Id))
                        {
                            member.User = user;
                            boardCache.Members.Add(member);
                        }
                        if (list is object)
                        {
                            if (!listCache.ContainsKey(list.Id))
                            {
                                listCache.Add(list.Id, list);
                            }
                            board.Lists.Add(list);
                        }
                        if (task is object)
                        {
                            task.List = list;
                            if (!taskCache.ContainsKey(task.Id))
                            {
                                taskCache.Add(task.Id, task);
                            }
                            KanbanTask cachedTask = taskCache[task.Id];
                            if (userId.HasValue && !cachedTask.Assignments.Any(assignedMember => assignedMember.User.Id == userId))
                            {
                                cachedTask.Assignments.Add(new BoardMember
                                {
                                    User = new User
                                    {
                                        Id = userId.Value
                                    }
                                });
                            }
                        }

                        return board;
                    },
                    queryParams,
                    splitOn: "id,isAdmin,id,id,userId");
            if (boardCache is null)
            {
                return boardCache;
            }

            IEnumerable<KanbanList> lists = listCache.Select(item => item.Value);
            IEnumerable<KanbanTask> tasks = taskCache.Select(item => item.Value);
            IEnumerable<KanbanList> boardLists = lists.GroupJoin(
                inner: tasks,
                outerKeySelector: list => list.Id,
                innerKeySelector: task => task.List.Id,
                resultSelector: (list, tasks) =>
                {
                    list.Tasks = tasks.ToList();
                    return list;
                });
            boardCache.Lists = boardLists.ToList();
            boardCache.Id = boardId;
            return boardCache;
        }

        public async Task<Board> Insert(Board board)
        {
            object queryParams = new
            {
                board.Id,
                board.Title,
                board.CreatedOn,
                board.ModifiedOn,
                CreatedBy = board.CreatedBy.Id
            };
            using IDbConnection connection = _connectionFactory.CreateConnection();
            int boardId = await connection.ExecuteScalarAsync<int>(InsertQuery, queryParams);
            return new Board
            {
                Id = boardId,
                Title = board.Title,
                CreatedBy = board.CreatedBy,
                CreatedOn = board.CreatedOn,
                ModifiedOn = board.ModifiedOn
            };
        }

        public async Task Remove(int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId
            };
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(DeleteBoardQuery, queryParams);
            transactionScope.Complete();
        }

        public async Task Update(Board board)
        {
            using IDbConnection connetion = _connectionFactory.CreateConnection();
            await connetion.ExecuteAsync(UpdateQuery, board);
        }
    }
}
