using System;
using KanbanBoard.WebApi.Models;
using KanbanBoard.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KanbanBoard.UnitTests.WebApi.Mocks
{
    public static class IBoardRepositoryMock
    {
        public static Mock<IBoardRepository> Mock() => new Mock<IBoardRepository>();

        public static Mock<IBoardRepository> MockExistsBoard(this Mock<IBoardRepository> mock, bool exists)
        {
            mock.Setup(repository => repository.Exists(It.IsAny<int>()))
                .ReturnsAsync(exists);
            return mock;
        }

        public static Mock<IBoardRepository> MockGetBoardByIdWithListsTasksAndMembers(
            this Mock<IBoardRepository> mock,
            Board returnValue)
        {
            mock
            .Setup(repository => repository.GetByIdWithListsTasksAndMembers(It.IsAny<int>()))
            .ReturnsAsync(returnValue);
            return mock;
        }
    }
}
