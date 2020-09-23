using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public class KanbanTaskRepository : RepositoryBase, IKanbanTaskRepository
    {
        private const string DeleteQuery = @"delete from assignments where taskId = @TaskId;
            delete from listTasks where taskId = @TaskId;
            delete from tasks where id = @TaskId";
        private const string GetByIdAndBoardIdQuery = @"select tasks.summary, tasks.description, tasks.tagColor,
            listTasks.listId, users.id, users.name, users.email from tasks
            left join assignments on assignments.taskId = tasks.id
            left join users on users.id = assignments.userId
            left join listTasks on listTasks.taskId = tasks.id
            where tasks.boardId = @BoardId and tasks.id = @TaskId";
        private const string InsertQuery = @"insert into tasks (summary, description, tagColor, createdOn, modifiedOn)
            values (@Summary, @Description, @TagColor, @CreatedOn, @ModifiedOn) returning id;";
        private const string InsertListTaskQuery = @"insert into listTasks (listId, taskId) values (@ListId, @TaskId);";

        public KanbanTaskRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
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
                splitOn: "listId,id"
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

            int taskId = await connection.ExecuteScalarAsync<int>(InsertQuery, task);
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
    }
}