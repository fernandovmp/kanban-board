using KanbanBoard.UnitTests.WebApi.Fakes;
using KanbanBoard.UnitTests.WebApi.Fakes.Repositories;
using KanbanBoard.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers.AssignmentsControllerTests
{
    public abstract class AssignmentsControllerTestsBase : ControllerTestsBase
    {
        protected readonly IBoardRepository fakeBoardRepository;
        protected readonly IBoardMemberRepository fakeMemberRepository;
        protected readonly IKanbanTaskRepository fakeTaskRepository;
        protected readonly IAssignmentRepository fakeAssignmentRepository;
        protected readonly IUrlHelper fakeUrlHelper;
        protected readonly ControllerContext fakeControllerContext;

        public AssignmentsControllerTestsBase()
        {
            fakeBoardRepository = new FakeBoardRepository();
            fakeAssignmentRepository = new FakeAssignmentRepository();
            fakeMemberRepository = new FakeBoardMemberRepository();
            fakeTaskRepository = new FakeKanbanTaskRepository();
            fakeUrlHelper = GetFakeUrlHelper(returnUrl: "Url");
            fakeControllerContext = GetFakeControlerContextWithFakeUser(identityName: "1");
        }
    }
}
