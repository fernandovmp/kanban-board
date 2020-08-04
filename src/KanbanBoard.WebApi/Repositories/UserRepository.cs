using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Services;

namespace KanbanBoard.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserRepository(IDbConnectionFactory connectionFactory, IDateTimeProvider dateTimeProvider)
        {
            _connectionFactory = connectionFactory;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<bool> ExistsUserWithEmail(string email)
        {
            string query = @"select 1 from users where email = @Email;";

            object queryParams = new
            {
                Email = email
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            bool exists = await connection.ExecuteScalarAsync<bool>(query, queryParams);

            return exists;
        }

        public async Task<User> Insert(User user)
        {
            string query = @"
            insert into users (name, email, password, createdOn, modifiedOn)
            values (@Name, @Email, @Password, @CreatedOn, @ModifiedOn) returning id;
            ";

            DateTime createdDate = _dateTimeProvider.UtcNow();

            object queryParams = new
            {
                user.Name,
                user.Email,
                user.Password,
                CreatedOn = createdDate,
                ModifiedOn = createdDate
            };

            using IDbConnection connection = _connectionFactory.CreateConnection();

            connection.Open();

            int userId = await connection.ExecuteScalarAsync<int>(query, queryParams);

            return new User
            {
                Id = userId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
        }
    }
}
