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
        private const string ExistsWithEmailQuery = @"select 1 from users where email = @Email;";
        private const string GetByIdQuery = @"select id, name, email from users where id = @Id;";
        private const string GetByEmailWithPasswordQuery = @"select id, name, email, password from users where email = @Email;";
        private const string InserQuery = @"insert into users (name, email, password, created_on, modified_on)
            values (@Name, @Email, @Password, @CreatedOn, @ModifiedOn) returning id;";
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserRepository(IDbConnectionFactory connectionFactory, IDateTimeProvider dateTimeProvider)
        {
            _connectionFactory = connectionFactory;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<bool> ExistsUserWithEmail(string email)
        {
            object queryParams = new
            {
                Email = email
            };
            using IDbConnection connection = _connectionFactory.CreateConnection();
            bool exists = await connection.ExecuteScalarAsync<bool>(ExistsWithEmailQuery, queryParams);
            return exists;
        }

        public async Task<User> GetByEmailWithPassword(string email)
        {
            object queryParams = new
            {
                Email = email
            };
            using IDbConnection connection = _connectionFactory.CreateConnection();
            User user = await connection.QueryFirstOrDefaultAsync<User>(GetByEmailWithPasswordQuery, queryParams);
            return user;
        }

        public async Task<User> GetById(int id)
        {
            object queryParams = new
            {
                Id = id
            };
            using IDbConnection connection = _connectionFactory.CreateConnection();
            User user = await connection.QueryFirstOrDefaultAsync<User>(GetByIdQuery, queryParams);
            return user;
        }

        public async Task<User> Insert(User user)
        {
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
            int userId = await connection.ExecuteScalarAsync<int>(InserQuery, queryParams);
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
