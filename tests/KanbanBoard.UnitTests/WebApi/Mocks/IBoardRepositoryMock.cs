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

        public static Mock<IBoardRepository> MockInsertKanbanTask(
            this Mock<IBoardRepository> mock,
            int withId) => mock.MockInsertKanbanTask(task => new KanbanTask
            {
                Id = withId,
                Board = task.Board,
                CreatedOn = task.CreatedOn,
                Description = task.Description,
                List = task.List,
                ModifiedOn = task.ModifiedOn,
                Summary = task.Summary,
                TagColor = task.TagColor
            });

        public static Mock<IBoardRepository> MockInsertKanbanTask(
            this Mock<IBoardRepository> mock,
            Func<KanbanTask, KanbanTask> returnValueFactory)
        {
            mock
                .Setup(repository => repository.InsertKanbanTask(It.IsAny<KanbanTask>()))
                .ReturnsAsync(returnValueFactory);
            return mock;
        }

        public static Mock<IBoardRepository> MockGetBoardList(
            this Mock<IBoardRepository> mock,
            KanbanList returnValue)
        {
            mock
            .Setup(repository => repository.GetBoardList(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(returnValue);
            return mock;
        }
    }
}
