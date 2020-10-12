using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
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
        protected IUrlHelper GetFakeUrlHelper(string returnUrl)
        {
            var actionContext = new ActionContext
            {
                ActionDescriptor = new ActionDescriptor(),
                RouteData = new RouteData(),
                HttpContext = new DefaultHttpContext()
            };
            var fakeUrlHelper = new Mock<IUrlHelper>();
            fakeUrlHelper
                .Setup(urlHelper => urlHelper.ActionContext)
                .Returns(actionContext);
            fakeUrlHelper
                .Setup(urlHelper => urlHelper.Action(It.IsAny<UrlActionContext>()))
                .Returns(returnUrl);
            return fakeUrlHelper.Object;
        }
    }
}
