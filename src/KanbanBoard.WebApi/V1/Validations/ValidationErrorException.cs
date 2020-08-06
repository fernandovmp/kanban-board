using System;
using System.Collections.Generic;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException(IEnumerable<ValidationError> errors, string message) : base(message)
        {
            Errors = errors;
        }
        public ValidationErrorException(IEnumerable<ValidationError> errors) : base()
        {
            Errors = errors;
        }
        public IEnumerable<ValidationError> Errors { get; }
    }
}
