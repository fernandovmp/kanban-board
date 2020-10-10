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
        private const string DeleteBoardQuery = @"delete from list_tasks where list_id in (select id from lists where board_id = @BoardId);
            delete from assignments where board_id = @BoardId;
            delete from board_members where board_id = @BoardId;
            delete from tasks where board_id = @BoardId;
            delete from lists where board_id = @BoardId;
            delete from boards where id = @BoardId;";
        private const string GetAllUserBoardsQuery = @"select boards.id, boards.title, boards.created_on, boards.modified_on from boards
            inner join board_members on board_members.board_id = boards.id
            where board_members.user_id = @UserId;";
        private const string GetByIdWithListsTasksAndMembersQuery = @"select boards.title, users.id, users.name, users.email, board_members.is_admin,
            lists.id, lists.title, tasks.id, tasks.summary, tasks.tag_color, assignments.user_id from boards
            left join board_members on board_members.board_id = boards.Id
            left join users on users.id = board_members.user_id
            left join lists on lists.board_id = boards.id
            left join list_tasks on list_tasks.list_id = lists.id
            left join tasks on list_tasks.task_id = tasks.id
            left join assignments on assignments.task_id = tasks.id
            where boards.id = @BoardId;";
        private const string InsertQuery = @"insert into boards (title, created_on, modified_on, created_by)
            values (@Title, @CreatedOn, @ModifiedOn, @CreatedBy) returning id;";
        private const string UpdateQuery = @"update boards set title = @Title, modified_on = @ModifiedOn where id = @Id;";
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
                    splitOn: "id,is_admin,id,id,user_id");
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
