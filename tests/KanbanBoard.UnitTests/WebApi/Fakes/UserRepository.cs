using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;

namespace KanbanBoard.UnitTests.WebApi.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public FakeUserRepository()
        {
            _users = new List<User>() {
                new User
                {
                    Id = 1,
                    Name = "Nero",
                    Email = "email@example.com",
                    Password = "SecretPassword"
                },
                new User {
                    Id = 2,
                    Name = "Kanban",
                    Email = "kanban@example.com",
                    Password = "SecretPassword"
                },
                new User {
                    Id = 3,
                    Name = "Default",
                    Email = "default@example.com",
                    Password = "SecretPassword"
                }
            };
        }

        public Task<bool> ExistsUserWithEmail(string email) => Async(_users.Exists(user => user.Email == email));

        public Task<User> GetByEmailWithPassword(string email) => Async(_users.FirstOrDefault(user => user.Email == email));
        public Task<User> GetById(int id) => Async(_users.FirstOrDefault(user => user.Id == id));

        public Task<User> Insert(User user) => Async(new User
        {
            Email = user.Email,
            Id = _users.Max(storedUser => storedUser.Id) + 1,
            Name = user.Name,
            Password = user.Password
        });

        private Task<T> Async<T>(T result) => Task.FromResult(result);
    }
}
