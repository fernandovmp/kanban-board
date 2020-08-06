//ValidationErrorMiddleware
using System.Net;
using System.Threading.Tasks;
using KanbanBoard.WebApi.V1.Validations;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace KanbanBoard.WebApi.V1.Middlewares
{
    public class ValidationErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationErrorException exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ValidationErrorException exception)
        {
            HttpStatusCode code = HttpStatusCode.BadRequest;

            var result = new ValidationErrorViewModel((int)code, "One or more validation errors occurred.", exception.Errors);
            string jsonResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(jsonResult);
        }
    }
}
