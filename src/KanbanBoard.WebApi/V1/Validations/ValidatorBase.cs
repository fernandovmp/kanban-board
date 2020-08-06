using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using KanbanBoard.WebApi.V1.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.V1.Validations
{
    public abstract class ValidatorBase<T> : AbstractValidator<T>, IValidatorInterceptor
    {
        public ValidationResult AfterMvcValidation(
            ControllerContext controllerContext,
            IValidationContext commonContext,
            ValidationResult result)
        {
            if (result.IsValid)
            {

                return result;
            }
            IEnumerable<ValidationError> errors = result.Errors
                .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage));
            throw new ValidationErrorException(errors);
        }

        public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
            => commonContext;
    }
}
