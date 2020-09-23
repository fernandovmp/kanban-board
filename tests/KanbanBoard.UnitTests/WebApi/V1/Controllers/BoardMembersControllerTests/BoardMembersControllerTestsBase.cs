using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.UnitTests.WebApi.Fakes.Repositories;
using KanbanBoard.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.BoardMembersControllerTests
{
    public abstract class BoardMembersControllerTestsBase : ControllerTestsBase
    {
        protected readonly IBoardRepository fakeBoardRepository;
        protected readonly IBoardMemberRepository fakeMemberRepository;
        protected readonly IUrlHelper fakeUrlHelper;
        protected readonly ControllerContext fakeControllerContext;

        public BoardMembersControllerTestsBase()
        {
            fakeBoardRepository = new FakeBoardRepository();
            fakeMemberRepository = new FakeBoardMemberRepository();
            fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }
    }
}
