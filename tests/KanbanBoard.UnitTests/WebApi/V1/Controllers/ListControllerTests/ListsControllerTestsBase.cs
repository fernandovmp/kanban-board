using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.UnitTests.WebApi.Fakes.Repositories;
using KanbanBoard.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.ListsControllerTests
{
    public abstract class ListsControllerTestsBase : ControllerTestsBase
    {
        protected readonly IBoardRepository fakeBoardRepository;
        protected readonly IBoardMemberRepository fakeMemberRepository;
        protected readonly IKanbanListRepository fakeListRepository;
        protected readonly IUrlHelper fakeUrlHelper;
        protected readonly ControllerContext fakeControllerContext;

        public ListsControllerTestsBase()
        {
            fakeBoardRepository = new FakeBoardRepository();
            fakeMemberRepository = new FakeBoardMemberRepository();
            fakeListRepository = new FakeKanbanListRepository();
            fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }
    }
}
