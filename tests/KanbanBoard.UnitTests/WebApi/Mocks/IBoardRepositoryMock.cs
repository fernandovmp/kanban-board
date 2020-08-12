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

        public static Mock<IBoardRepository> MockGetBoardMember(this Mock<IBoardRepository> mock, BoardMember returnValue)
        {
            mock
                .Setup(repository => repository.GetBoardMember(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(returnValue);
            return mock;
        }

        public static Mock<IBoardRepository> MockGetBoardMember(this Mock<IBoardRepository> mock, bool isAdmin) =>
            mock.MockGetBoardMember(new BoardMember
            {
                IsAdmin = isAdmin
            });

        public static Mock<IBoardRepository> MockExistsBoard(this Mock<IBoardRepository> mock, bool exists)
        {
            mock.Setup(repository => repository.ExistsBoard(It.IsAny<int>()))
                .ReturnsAsync(exists);
            return mock;
        }

        public static Mock<IBoardRepository> MockInsertKanbanList(
            this Mock<IBoardRepository> mock,
            Func<KanbanList, KanbanList> returnValueFactory)
        {
            mock
                .Setup(repository => repository.InsertKanbanList(It.IsAny<KanbanList>()))
                .ReturnsAsync(returnValueFactory);
            return mock;
        }
    }
}
