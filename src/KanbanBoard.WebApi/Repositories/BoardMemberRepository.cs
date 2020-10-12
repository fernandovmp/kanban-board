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
    public class BoardMemberRepository : RepositoryBase, IBoardMemberRepository
    {
        private const string CountQuery = @"select Count(*) from board_members where board_id = @BoardId;";
        private const string GetAllMembersOfTheBoardQuery = @"select users.id, users.name, users.email, board_members.is_admin from board_members
            inner join users on users.id = board_members.user_id
            where board_members.board_id = @BoardId;";
        private const string GetByBoardIdAndUserIdQuery = @"select is_admin from board_members where board_id = @BoardId and user_id = @UserId;";
        private const string InsertQuery = @"insert into board_members (board_id, user_id, is_admin, created_on, modified_on)
            values (@BoardId, @UserId, @IsAdmin, @CreatedOn, @ModifiedOn);";
        private const string RemoveQuery = @"delete from assignments where board_id = @BoardId and user_id = @UserId;
            delete from board_members where board_id = @BoardId and user_id = @UserId;";

        public BoardMemberRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<int> CountMembers(int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId
            };

            using IDbConnection connection = connectionFactory.CreateConnection();
            int count = await connection.ExecuteScalarAsync<int>(CountQuery, queryParams);
            return count;
        }

        public async Task<IEnumerable<BoardMember>> GetAllMembersOfTheBoard(int boardId)
        {
            object queryParams = new
            {
                BoardId = boardId
            };

            using IDbConnection connection = connectionFactory.CreateConnection();
            IEnumerable<BoardMember> boardMembers = await connection.QueryAsync<User, BoardMember, BoardMember>(
                GetAllMembersOfTheBoardQuery,
                map: (user, member) =>
                {
                    member.User = user;
                    return member;
                },
                queryParams,
                splitOn: "id,is_admin"
            );

            return boardMembers;
        }

        public async Task<BoardMember> GetByBoardIdAndUserId(int boardId, int userId)
        {
            object queryParams = new
            {
                BoardId = boardId,
                UserId = userId
            };

            using IDbConnection connection = connectionFactory.CreateConnection();
            BoardMember boardMember = await connection.QueryFirstOrDefaultAsync<BoardMember>(GetByBoardIdAndUserIdQuery, queryParams);
            if (boardMember is object)
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

        public async Task Insert(BoardMember boardMember)
        {
            object queryParams = new
            {
                BoardId = boardMember.Board.Id,
                UserId = boardMember.User.Id,
                boardMember.IsAdmin,
                boardMember.CreatedOn,
                boardMember.ModifiedOn
            };

            using IDbConnection connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(InsertQuery, queryParams);
        }

        public async Task Remove(BoardMember boardMember)
        {
            object queryParams = new
            {
                UserId = boardMember.User.Id,
                BoardId = boardMember.Board.Id
            };

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using IDbConnection connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(RemoveQuery, queryParams);
            transactionScope.Complete();
        }
    }
}
