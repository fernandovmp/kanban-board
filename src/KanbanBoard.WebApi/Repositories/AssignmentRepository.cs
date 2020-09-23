using System.Data;
using System.Threading.Tasks;
using Dapper;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public class AssignmentRepository : RepositoryBase, IAssignmentRepository
    {
        private const string DeleteQuery = @"delete from assignments where userId = @UserId and taskId = @TaskId and boardId = @BoardId;";
        private const string InsertQuery = @"insert into assignments (boardId, userId, taskId) values (@BoardId, @UserId, @TaskId);";
        private const string ExistsQuery = @"select 1 from assignments where taskId = @TaskId and boardId = @BoardId and userId = @UserId;";

        public AssignmentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<bool> ExistsAssignment(int taskId, BoardMember member)
        {
            object queryParams = new
            {
                BoardId = member.Board.Id,
                UserId = member.User.Id,
                TaskId = taskId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            bool exists = await connection.ExecuteScalarAsync<bool>(ExistsQuery, queryParams);
            return exists;
        }

        public async Task Insert(int taskId, BoardMember member)
        {
            object queryParams = new
            {
                BoardId = member.Board.Id,
                UserId = member.User.Id,
                TaskId = taskId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(InsertQuery, queryParams);
        }

        public async Task Remove(int taskId, BoardMember boardMember)
        {
            object queryParams = new
            {
                BoardId = boardMember.Board.Id,
                UserId = boardMember.User.Id,
                TaskId = taskId
            };
            using IDbConnection connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(DeleteQuery, queryParams);
        }
    }
}
