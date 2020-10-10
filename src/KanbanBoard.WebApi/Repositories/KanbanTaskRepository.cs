using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories.QueryBuilder;
using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public class KanbanTaskRepository : RepositoryBase, IKanbanTaskRepository
    {
        private const string DeleteQuery = @"delete from assignments where task_id = @TaskId;
            delete from list_tasks where task_id = @TaskId;
            delete from tasks where id = @TaskId";
        private const string GetAllTasksOfTheBoardQuery = @"select tasks.id, tasks.summary, tasks.description, tasks.tag_color,
            listTasks.list_id, assignments.user_id from tasks
            left join assignments on assignments.task_id = tasks.id
            left join list_tasks on list_tasks.task_id = tasks.id
            where tasks.board_id = @BoardId";
        private const string GetByIdAndBoardIdQuery = @"select tasks.summary, tasks.description, tasks.tag_color,
            list_tasks.list_id, users.id, users.name, users.email from tasks
            left join assignments on assignments.task_id = tasks.id
            left join users on users.id = assignments.user_id
            left join list_tasks on list_tasks.task_id = tasks.id
            where tasks.board_id = @BoardId and tasks.id = @TaskId";
        private const string InsertQuery = @"insert into tasks (summary, description, tag_color, board_id, created_on, modified_on)
            values (@Summary, @Description, @TagColor, @BoardId, @CreatedOn, @ModifiedOn) returning id;";
        private const string InsertListTaskQuery = @"insert into list_tasks (list_id, task_id) values (@ListId, @TaskId);";

        public KanbanTaskRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<KanbanTask>> GetAllTasksOfTheBoard(int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();

            var taskCache = new Dictionary<int, KanbanTask>();
            _ = await connection.QueryAsync<KanbanTask, int, int?, KanbanTask>(
                GetAllTasksOfTheBoardQuery,
                map: (task, listId, assignedUserId) =>
                {
                    if (!taskCache.ContainsKey(task.Id))
                    {
                        task.List = new KanbanList
                        {
                            Id = listId
                        };
                        taskCache.Add(task.Id, task);
                    }
                    KanbanTask kanbanTask = taskCache[task.Id];
                    if (assignedUserId.HasValue)
                    {
                        kanbanTask.Assignments.Add(new BoardMember
                        {
                            User = new User
                            {
                                Id = assignedUserId.Value
                            }
                        });

                    }
                    return task;
                },
                queryParams,
                splitOn: "id,list_id,user_id"
            );

            return taskCache.Select(task => task.Value);

        }

        public async Task<KanbanTask> GetByIdAndBoardId(int taskId, int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId,
                TaskId = taskId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();

            KanbanTask kanbanTask = null;
            _ = await connection.QueryAsync<KanbanTask, int, User, KanbanTask>(
                GetByIdAndBoardIdQuery,
                map: (task, listId, user) =>
                {
                    if (kanbanTask is null)
                    {
                        kanbanTask = task;
                        kanbanTask.List = new KanbanList
                        {
                            Id = listId
                        };
                    }
                    if (user is object)
                    {
                        kanbanTask.Assignments.Add(new BoardMember
                        {
                            User = user
                        });

                    }
                    return task;
                },
                queryParams,
                splitOn: "list_id,id"
            );

            if (kanbanTask is object)
            {
                kanbanTask.Id = taskId;
                kanbanTask.Board = new Board
                {
                    Id = boardId
                };
            }

            return kanbanTask;
        }

        public async Task<KanbanTask> Insert(KanbanTask task)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = connectionFactory.CreateConnection();

            object insertParams = new
            {
                task.Summary,
                task.Description,
                task.TagColor,
                BoardId = task.Board.Id,
                task.CreatedOn,
                task.ModifiedOn
            };
            int taskId = await connection.ExecuteScalarAsync<int>(InsertQuery, insertParams);
            object queryParams = new
            {
                TaskId = taskId,
                ListId = task.List.Id
            };
            await connection.ExecuteAsync(InsertListTaskQuery, queryParams);
            transactionScope.Complete();

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

        public async Task Remove(KanbanTask task)
        {
            object queryParams = new
            {
                TaskId = task.Id
            };
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(DeleteQuery, queryParams);
            transactionScope.Complete();
        }

        public async Task Update(IPatchQueryBuilder<PatchTaskParams> patchTaskQueryBuilder)
        {
            (string query, PatchTaskParams queryParams) = patchTaskQueryBuilder.Build();
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(query, queryParams);
            transactionScope.Complete();
        }
    }
}
