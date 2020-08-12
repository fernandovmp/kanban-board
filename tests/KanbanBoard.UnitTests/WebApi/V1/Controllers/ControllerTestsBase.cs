using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KanbanBoard.UnitTests.WebApi.V1.Controllers
{
    public class ControllerTestsBase
    {
        protected ControllerContext GetFakeControlerContextWithFakeUser(string identityName)
        {
            var fakeHttpContext = new Mock<HttpContext>();
            fakeHttpContext
                .Setup(httpContext => httpContext.User.Identity.Name)
                .Returns(identityName);
            var fakeControllerContext = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };
            return fakeControllerContext;
        }
    }
}
