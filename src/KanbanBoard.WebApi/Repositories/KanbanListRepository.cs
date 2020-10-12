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
    public class KanbanListRepository : RepositoryBase, IKanbanListRepository
    {
        private const string GetAllListsOfTheBoardQuery = @"select lists.id, lists.title, lists.created_on, lists.modified_on, listTasks.task_id
            from lists
            left join list_tasks on list_tasks.list_id = lists.id
            where lists.board_id = @BoardId;";
        private const string GetByIdAndBoardIdWithTasksQuery = @"select lists.title, tasks.id, tasks.summary, tasks.description, tasks.tag_color, assignments.user_id
            from lists
            left join list_tasks on list_tasks.list_id = lists.id
            left join tasks on tasks.id = list_tasks.task_id
            left join assignments on assignments.task_Id = tasks.id
            where lists.id = @ListId and lists.board_id = @BoardId;";
        private const string InsertQuery = @"insert into lists (board_id, title, created_on, modified_on)
            values (@BoardId, @Title, @CreatedOn, @ModifiedOn) returning id;";
        private const string GetByIdAndBoardIdQuery = @"select title from lists where board_id = @BoardId and id = @ListId";
        private const string UpdateQuery = @"update lists set title = @Title, modified_on = @ModifiedOn where id = @Id;";

        public KanbanListRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<KanbanList>> GetAllListsOfTheBoard(int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            var listCache = new Dictionary<int, KanbanList>();
            _ = await connection.QueryAsync<KanbanList, int?, KanbanList>(
                GetAllListsOfTheBoardQuery,
                map: (list, taskId) =>
                {
                    if (!listCache.ContainsKey(list.Id))
                    {
                        listCache.Add(list.Id, list);
                    }
                    if (taskId.HasValue)
                    {
                        KanbanList kanbanList = listCache[list.Id];
                        kanbanList.Tasks.Add(new KanbanTask
                        {
                            Id = taskId.Value
                        });
                    }
                    return list;
                },
                queryParams,
                splitOn: "task_id");
            return listCache.Select(list => list.Value);
        }

        public async Task<KanbanList> GetByIdAndBoardId(int listId, int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId,
                ListId = listId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            KanbanList list = await connection.QueryFirstOrDefaultAsync<KanbanList>(GetByIdAndBoardIdQuery, queryParams);
            if (list is object)
            {
                list.Id = listId;
                list.Board = new Board
                {
                    Id = boardId
                };
            }
            return list;
        }

        public async Task<KanbanList> GetByIdAndBoardIdWithTasks(int listId, int boardId)
        {
            object queryParams = new
            {
                ListId = listId,
                BoardId = boardId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            KanbanList listCache = null;
            var taskCache = new Dictionary<int, KanbanTask>();
            _ = await connection.QueryAsync<KanbanList, KanbanTask, int?, KanbanList>(
                GetByIdAndBoardIdWithTasksQuery,
                map: (list, task, assignedMemberId) =>
                {
                    if (listCache is null)
                    {
                        listCache = list;
                    }
                    if (task is object)
                    {
                        if (!taskCache.ContainsKey(task.Id))
                        {
                            taskCache.Add(task.Id, task);
                        }
                        if (assignedMemberId.HasValue)
                        {
                            KanbanTask kanbanTask = taskCache[task.Id];
                            kanbanTask.Assignments.Add(new BoardMember
                            {
                                User = new User
                                {
                                    Id = assignedMemberId.Value
                                }
                            });
                        }
                    }
                    return list;
                },
                queryParams,
                splitOn: "title,id,user_id");

            if (listCache is object)
            {
                listCache.Id = listId;
                listCache.Tasks = taskCache.Select(task => task.Value).ToList();
            }
            return listCache;
        }

        public async Task<KanbanList> Insert(KanbanList list)
        {
            object queryParams = new
            {
                BoardId = list.Board.Id,
                list.Title,
                list.CreatedOn,
                list.ModifiedOn
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            int listId = await connection.ExecuteScalarAsync<int>(InsertQuery, queryParams);
            return new KanbanList
            {
                Id = listId,
                Title = list.Title,
                Board = list.Board,
                CreatedOn = list.CreatedOn,
                ModifiedOn = list.ModifiedOn
            };
        }

        public async Task Remove(KanbanList list)
        {
            const string GetTasksQuery = @"select task_id from list_tasks where list_id = @ListId;";
            object getTasksQueryParams = new
            {
                ListId = list.Id,
            };

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = connectionFactory.CreateConnection();
            IEnumerable<int> tasks = await connection.QueryAsync<int>(GetTasksQuery, getTasksQueryParams);

            string deleteQuery = @"delete from lists where id = @ListId and board_id = @BoardId";
            if (tasks.Count() > 0)
            {
                deleteQuery = @"delete from assignments where task_id = any (@Tasks);
                delete from list_tasks where list_id = @ListId;
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

        public async Task Update(KanbanList list)
        {
            using IDbConnection connetion = connectionFactory.CreateConnection();
            await connetion.ExecuteAsync(UpdateQuery, list);
        }
    }
}
