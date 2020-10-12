using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.UnitTests.WebApi.Fakes.Repositories;
using KanbanBoard.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.BoardsControllerTests
{
    public abstract class BoardsControllerTestsBase : ControllerTestsBase
    {
        protected readonly IBoardRepository fakeBoardRepository;
        protected readonly IBoardMemberRepository fakeMemberRepository;
        protected readonly IUrlHelper fakeUrlHelper;
        protected readonly ControllerContext fakeControllerContext;

        public BoardsControllerTestsBase()
        {
            fakeBoardRepository = new FakeBoardRepository();
            fakeMemberRepository = new FakeBoardMemberRepository();
            fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }
    }
}
