using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            using IDbConnection connection = _connectionFactory.CreateConnection();

            IEnumerable<Board> boards = await connection
                .QueryAsync<Board, User, BoardMember, KanbanList, KanbanTask, int?, Board>(
                    query,
                    map: (board, user, member, list, task, userId) =>
                    {
                        if (member is { })
                        {
                            member.User = user;
                        }
                        board.Members.Add(member);
                        if (list is { })
                        {
                            if (task is { } && userId.HasValue)
                            {
                                task.Assignments.Add(new BoardMember
                                {
                                    User = new User
                                    {
                                        Id = userId.Value
                                    }
                                });
                            }
                            list.Tasks.Add(task);
                        }
                        board.Lists.Add(list);
                        return board;
                    },
                    queryParams,
                    splitOn: "id,isAdmin,id,id,userId");
            Board result = boards
                .GroupBy(board => board.Id)
                .Select(boardsGroup =>
                {
                    Board board = boardsGroup.FirstOrDefault();
                    if (board is { })
                    {
                        board.Members = boardsGroup
                            .Select(_board => _board.Members.Single())
                            .Where(member => member is { })
                            .GroupBy(member => member.User.Id)
                            .Select(memberGroup => memberGroup.FirstOrDefault())
                            .ToList();
                        board.Lists = boardsGroup
                            .Select(_board => _board.Lists.Single())
                            .Where(list => list is { })
                            .GroupBy(list => list.Id)
                            .Select(listsGroup =>
                            {
                                KanbanList list = listsGroup.FirstOrDefault();
                                if (list is { })
                                {
                                    list.Tasks = listsGroup
                                        .Select(_list => _list.Tasks.Single())
                                        .Where(task => task is { })
                                        .ToList();
                                }
                                return list;
                            })
                            .ToList();
                    }
                    return board;
                })
                .FirstOrDefault();
            if (result is { })
            {
                result.Id = boardId;
            }
            return result;
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
            string query = @"insert into tasks (summary, description, tagColor, createdOn, modifiedOn)
                values (@Summary, @Description, @TagColor, @CreatedOn, @ModifiedOn)
                returning id;";

            using IDbConnection connection = _connectionFactory.CreateConnection();

            int taskId = await connection.ExecuteScalarAsync<int>(query, task);

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

        public async Task Update(Board board)
        {
            string query = @"update boards set title = @Title, modifiedOn = @ModifiedOn where id = @Id;";
            using IDbConnection connetion = _connectionFactory.CreateConnection();

            await connetion.ExecuteAsync(query, board);
        }
    }
}
