using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.UnitTests.WebApi.Fakes.Repositories;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using KanbanBoard.WebApi.V1.Controllers;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.TasksControllerTests
{
    public abstract class TaskControllerTestsBase : ControllerTestsBase
    {
        protected readonly IBoardRepository fakeBoardRepository;
        protected readonly IBoardMemberRepository fakeMemberRepository;
        protected readonly IKanbanTaskRepository fakeTaskRepository;
        protected readonly IKanbanListRepository fakeListRepository;
        protected readonly IUrlHelper fakeUrlHelper;
        protected readonly ControllerContext fakeControllerContext;

        public TaskControllerTestsBase()
        {
            fakeBoardRepository = new FakeBoardRepository();
            fakeListRepository = new FakeKanbanListRepository();
            fakeMemberRepository = new FakeBoardMemberRepository();
            fakeTaskRepository = new FakeKanbanTaskRepository();
            fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }
    }
}
