using System.Collections.Generic;

namespace KanbanBoard.WebApi.V1.ViewModels
{
    public class ValidationError
    {
        public ValidationError(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; }
        public string Message { get; }
    }
    public class ValidationErrorViewModel : ErrorViewModel
    {
        public ValidationErrorViewModel(int status, string message, IEnumerable<ValidationError> errors) : base(status, message)
        {
            Errors = errors;
        }

        public IEnumerable<ValidationError> Errors { get; }
    }
}
