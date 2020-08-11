using System.Collections.Generic;
using System.Data;
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
    }
}
