using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Controllers
{
    public abstract class V1ControllerBase : ControllerBase
    {
        public BadRequestObjectResult V1BadRequest(string message) =>
            BadRequest(new ErrorViewModel(400, message));

        public NotFoundObjectResult V1NotFound(string message) =>
            NotFound(new ErrorViewModel(404, message));

        public ConflictObjectResult V1Conflict(string message) => Conflict(new ErrorViewModel(409, message));
    }
}
