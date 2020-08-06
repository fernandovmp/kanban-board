
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static BadRequestObjectResult V1BadRequest(this ControllerBase controller, string message) =>
            controller.BadRequest(new ErrorViewModel(400, message));

        public static NotFoundObjectResult V1NotFound(this ControllerBase controller, string message) =>
            controller.NotFound(new ErrorViewModel(404, message));
    }
}
