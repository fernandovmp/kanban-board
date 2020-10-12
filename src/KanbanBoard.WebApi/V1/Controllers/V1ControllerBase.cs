using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{
    public abstract class V1ControllerBase : ControllerBase
    {
        [NonAction]
        public BadRequestObjectResult V1BadRequest(string message) =>
            BadRequest(new ErrorViewModel(400, message));

        [NonAction]
        public NotFoundObjectResult V1NotFound(string message) =>
            NotFound(new ErrorViewModel(404, message));

        [NonAction]
        public ConflictObjectResult V1Conflict(string message) => Conflict(new ErrorViewModel(409, message));
    }
}
