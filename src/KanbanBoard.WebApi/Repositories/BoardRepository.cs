using System;
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
        private readonly IDbConnectionFactory _connectionFactory;

        public BoardRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CountBoardMembers(int boardId)
        {
            string query = @"select Count(*) from boardMembers where boardId = @BoardId;";
            object queryParams = new
            {
                BoardId = boardId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            int count = await connection.ExecuteScalarAsync<int>(query, queryParams);
            return count;
        }

        public async Task CreateAssignment(int taskId, BoardMember member)
        {
            string query = @"insert into assignments (boardId, userId, taskId) values (@BoardId, @UserId, @TaskId);";
            object queryParams = new
            {
                BoardId = member.Board.Id,
                UserId = member.User.Id,
                TaskId = taskId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, queryParams);
        }

        public async Task<bool> ExistsAssignment(int taskId, BoardMember member)
        {
            string query = @"select 1 from assignments where taskId = @TaskId and boardId = @BoardId and userId = @UserId;";
            object queryParams = new
            {
                BoardId = member.Board.Id,
                UserId = member.User.Id,
                TaskId = taskId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            bool exists = await connection.ExecuteScalarAsync<bool>(query, queryParams);
            return exists;
        }

        public async Task<bool> ExistsBoard(int boardId)
        {
            string query = @"select 1 from boards where id = @Id;";
            object queryParams = new
            {
                Id = boardId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            bool exists = await connection.ExecuteScalarAsync<bool>(query, queryParams);

            return exists;
        }

        public async Task<IEnumerable<BoardMember>> GetAllBoardMembers(int boardId)
        {
            string query = @"select users.id, users.name, users.email, boardMembers.isAdmin from boardMembers
            inner join users on users.id = boardMembers.userId
            where boardMembers.boardId = @BoardId";
            object queryParams = new
            {
                BoardId = boardId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            IEnumerable<BoardMember> boardMembers = await connection.QueryAsync<User, BoardMember, BoardMember>(
                query,
                map: (user, member) =>
                {
                    member.User = user;
                    return member;
                },
                queryParams,
                splitOn: "id,isAdmin"
            );

            return boardMembers;
        }

        public async Task<IEnumerable<Board>> GetAllUserBoards(int userId)
        {
            string query = @"
            select boards.id, boards.title, boards.createdOn, boards.modifiedOn from boards
                inner join boardmembers on boardmembers.boardId = boards.id
                where boardmembers.userId = @UserId;";
            object queryParams = new
            {
                UserId = userId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            IEnumerable<Board> boards = await connection.QueryAsync<Board>(query, queryParams);

            return boards;
        }

        public async Task<Board> GetBoardByIdWithListsTasksAndMembers(int boardId)
        {
            string query = @"
            select boards.title, users.id, users.name, users.email, boardMembers.isAdmin,
            lists.id, lists.title, tasks.id, tasks.summary, tasks.tagColor, assignments.userId from boards
            left join boardMembers on boardMembers.boardId = boards.Id
            left join users on users.id = boardMembers.userId
            left join lists on lists.boardId = boards.id
            left join listTasks on listTasks.listId = lists.id
            left join tasks on listTasks.taskId = tasks.id
            left join assignments on assignments.taskId = tasks.id
            where boards.id = @BoardId;
            ";
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
                    query,
                    map: (board, user, member, list, task, userId) =>
                    {
                        if (boardCache is null)
                        {
                            boardCache = board;
                        }
                        if (member is { } && !boardCache.Members.Any(_member => _member.User.Id == user.Id))
                        {
                            member.User = user;
                            boardCache.Members.Add(member);
                        }
                        if (list is { })
                        {
                            if (!listCache.ContainsKey(list.Id))
                            {
                                listCache.Add(list.Id, list);
                            }
                            board.Lists.Add(list);
                        }
                        if (task is { })
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

        public async Task<KanbanList> GetBoardList(int boardId, int listId)
        {
            string query = @"select title from lists where boardId = @BoardId and id = @ListId";
            object queryParams = new
            {
                BoardId = boardId,
                ListId = listId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            KanbanList list = await connection.QueryFirstOrDefaultAsync<KanbanList>(query, queryParams);
            if (list is { })
            {
                list.Id = listId;
                list.Board = new Board
                {
                    Id = boardId
                };
            }

            return list;
        }

        public async Task<BoardMember> GetBoardMember(int boardId, int userId)
        {
            string query = @"select isAdmin from boardMembers where boardId = @BoardId and userId = @UserId";
            object queryParams = new
            {
                BoardId = boardId,
                UserId = userId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            BoardMember boardMember = await connection.QueryFirstOrDefaultAsync<BoardMember>(query, queryParams);
            if (boardMember is { })
            {
                boardMember.Board = new Board
                {
                    Id = boardId
                };
                boardMember.User = new User
                {
                    Id = userId
                };
            }

            return boardMember;
        }

        public async Task<KanbanTask> GetBoardTask(int boardId, int taskId)
        {
            string query = @"select tasks.summary, tasks.description, tasks.tagColor,
            listTasks.listId, users.id, users.name, users.email from tasks
            left join assignments on assignments.taskId = tasks.id
            left join users on users.id = assignments.userId
            left join listTasks on listTasks.taskId = tasks.id
            where tasks.boardId = @BoardId and tasks.id = @TaskId";

            object queryParams = new
            {
                BoardId = boardId,
                TaskId = taskId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            IEnumerable<KanbanTask> tasks = await connection.QueryAsync<KanbanTask, int, User, KanbanTask>(
                query,
                map: (task, listId, user) =>
                {
                    task.List = new KanbanList
                    {
                        Id = listId
                    };
                    task.Assignments.Add(new BoardMember
                    {
                        User = user
                    });
                    return task;
                },
                queryParams,
                splitOn: "listId,id");
            KanbanTask result = tasks
                .GroupBy(task => taskId)
                .Select(taskGroup =>
                {
                    KanbanTask task = taskGroup.FirstOrDefault();
                    if (task is { })
                    {
                        task.Assignments = taskGroup
                            .Select(_task => _task.Assignments.Single())
                            .Where(member => member.User is { })
                            .GroupBy(member => member.User.Id)
                            .Select(memberGroup => memberGroup.FirstOrDefault())
                            .ToList();
                    }
                    return task;
                })
                .FirstOrDefault();
            if (result is { })
            {
                result.Id = taskId;
                result.Board = new Board
                {
                    Id = boardId
                };
            }

            return result;
        }

        public async Task<Board> Insert(Board board)
        {
            string query = @"insert into boards (title, createdOn, modifiedOn, createdBy)
                values (@Title, @CreatedOn, @ModifiedOn, @CreatedBy) returning id;";

            object queryParams = new
            {
                board.Id,
                board.Title,
                board.CreatedOn,
                board.ModifiedOn,
                CreatedBy = board.CreatedBy.Id
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            int boardId = await connection.ExecuteScalarAsync<int>(query, queryParams);

            return new Board
            {
                Id = boardId,
                Title = board.Title,
                CreatedBy = board.CreatedBy,
                CreatedOn = board.CreatedOn,
                ModifiedOn = board.ModifiedOn
            };
        }

        public async Task InsertBoardMember(BoardMember boardMember)
        {
            string query = @"insert into boardMembers (boardId, userId, isAdmin, createdOn, modifiedOn)
                values (@BoardId, @UserId, @IsAdmin, @CreatedOn, @ModifiedOn);";

            object queryParams = new
            {
                BoardId = boardMember.Board.Id,
                UserId = boardMember.User.Id,
                boardMember.IsAdmin,
                boardMember.CreatedOn,
                boardMember.ModifiedOn
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            await connection.ExecuteAsync(query, queryParams);
        }

        public async Task<KanbanList> InsertKanbanList(KanbanList list)
        {
            string query = @"insert into lists (boardId, title, createdOn, modifiedOn)
                values (@BoardId, @Title, @CreatedOn, @ModifiedOn) returning id;";
            object queryParams = new
            {
                BoardId = list.Board.Id,
                list.Title,
                list.CreatedOn,
                list.ModifiedOn
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            int listId = await connection.ExecuteScalarAsync<int>(query, queryParams);

            return new KanbanList
            {
                Id = listId,
                Title = list.Title,
                Board = list.Board,
                CreatedOn = list.CreatedOn,
                ModifiedOn = list.ModifiedOn
            };
        }

        public async Task<KanbanTask> InsertKanbanTask(KanbanTask task)
        {
            string query = @"insert into tasks (summary, description, tagColor, boardId, createdOn, modifiedOn)
                values (@Summary, @Description, @TagColor, @BoardId, @CreatedOn, @ModifiedOn)
                returning id;";
            object insertParams = new
            {
                task.Summary,
                task.Description,
                task.TagColor,
                task.CreatedOn,
                task.ModifiedOn,
                BoardId = task.Board.Id
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            int taskId = await connection.ExecuteScalarAsync<int>(query, insertParams);

            string taskListQuery = @"insert into listTasks (listId, taskId) values (@ListId, @TaskId);";
            object queryParams = new
            {
                TaskId = taskId,
                ListId = task.List.Id
            };

            await connection.ExecuteAsync(taskListQuery, queryParams);

            return new KanbanTask
            {
                Id = taskId,
                Summary = task.Summary,
                Description = task.Description,
                TagColor = task.TagColor,
                Board = task.Board,
                List = task.List,
                CreatedOn = task.CreatedOn,
                ModifiedOn = task.ModifiedOn
            };
        }

        public async Task RemoveAssignment(int taskId, BoardMember boardMember)
        {
            string query = @"delete from assignments where userId = @UserId and taskId = @TaskId and boardId = @BoardId;";
            object queryParams = new
            {
                BoardId = boardMember.Board.Id,
                UserId = boardMember.User.Id,
                TaskId = taskId
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, queryParams);
        }

        public async Task RemoveBoardMember(BoardMember boardMember)
        {
            string query = @"delete from assignments where boardId = @BoardId and userId = @UserId;
            delete from boardMembers where boardId = @BoardId and userId = @UserId;";
            object queryParams = new
            {
                UserId = boardMember.User.Id,
                BoardId = boardMember.Board.Id
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, queryParams);
        }

        public async Task RemoveList(KanbanList list)
        {
            string getTasksQuery = @"select taskId from listTasks where listId = @ListId;";
            object getTasksQueryParams = new
            {
                ListId = list.Id,
            };

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = _connectionFactory.CreateConnection();
            IEnumerable<int> tasks = await connection.QueryAsync<int>(getTasksQuery, getTasksQueryParams);

            string deleteQuery = @"delete from lists where id = @ListId and boardId = @BoardId";
            if (tasks.Count() > 0)
            {
                deleteQuery = @"delete from assignments where taskId = any (@Tasks);
                delete from listTasks where listId = @ListId;
                delete from tasks where id = any (@Tasks);"
                + deleteQuery;
            }

            object deleteQueryParams = new
            {
                BoardId = list.Board.Id,
                ListId = list.Id,
                Tasks = tasks
            };

            await connection.ExecuteAsync(deleteQuery, deleteQueryParams);
            transactionScope.Complete();
        }

        public async Task RemoveTask(KanbanTask task)
        {
            string query = @"delete from assignments where taskId = @TaskId;
            delete from listTasks where taskId = @TaskId;
            delete from tasks where id = @TaskId";
            object queryParams = new
            {
                TaskId = task.Id
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, queryParams);
        }

        public async Task Update(Board board)
        {
            string query = @"update boards set title = @Title, modifiedOn = @ModifiedOn where id = @Id;";
            using IDbConnection connetion = _connectionFactory.CreateConnection();

            await connetion.ExecuteAsync(query, board);
        }

        public async Task UpdateKanbanList(KanbanList list)
        {
            string query = @"update lists set title = @Title, modifiedOn = @ModifiedOn where id = @Id;";
            using IDbConnection connetion = _connectionFactory.CreateConnection();

            await connetion.ExecuteAsync(query, list);
        }
    }
}
