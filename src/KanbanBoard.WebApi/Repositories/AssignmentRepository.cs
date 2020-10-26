using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public class AssignmentRepository : RepositoryBase, IAssignmentRepository
    {
        private const string DeleteQuery = @"delete from assignments where user_id = @UserId and task_id = @TaskId and board_id = @BoardId;";
        private const string InsertQuery = @"insert into assignments (board_id, user_id, task_id) values (@BoardId, @UserId, @TaskId);";
        private const string ExistsQuery = @"select 1 from assignments where task_id = @TaskId and board_id = @BoardId and user_id = @UserId;";

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

        public Task Insert(int taskId, BoardMember member) => Insert(taskId, new BoardMember[] { member });

        public async Task Insert(int taskId, IEnumerable<BoardMember> members)
        {
            if (members.Count() == 0)
            {
                return;
            }
            IEnumerable queryParams = members.Select(member => new
            {
                BoardId = member.Board.Id,
                UserId = member.User.Id,
                TaskId = taskId
            });
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
