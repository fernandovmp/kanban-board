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
        private const string GetAllListsOfTheBoardQuery = @"select lists.id, lists.title, lists.createdOn, lists.modifiedOn, listTasks.taskId
            from lists
            left join listTasks on listTasks.listId = lists.id
            where lists.boardId = @BoardId;";
        private const string InsertQuery = @"insert into lists (boardId, title, createdOn, modifiedOn)
            values (@BoardId, @Title, @CreatedOn, @ModifiedOn) returning id;";
        private const string GetByIdAndBoardIdQuery = @"select title from lists where boardId = @BoardId and id = @ListId";
        private const string UpdateQuery = @"update lists set title = @Title, modifiedOn = @ModifiedOn where id = @Id;";

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
                splitOn: "taskId");
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
            string getTasksQuery = @"select taskId from listTasks where listId = @ListId;";
            object getTasksQueryParams = new
            {
                ListId = list.Id,
            };

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = connectionFactory.CreateConnection();
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

        public async Task Update(KanbanList list)
        {
            using IDbConnection connetion = connectionFactory.CreateConnection();
            await connetion.ExecuteAsync(UpdateQuery, list);
        }
    }
}
