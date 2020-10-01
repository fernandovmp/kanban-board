using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using KanbanBoard.WebApi.V1.Validations;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.Extensions
{
    public static class ValidationExtensions
    {
        public static void ValidateAndEnsureModelStateIsValid<T>(
            this AbstractValidator<T> validator, T model)
        {
            ValidationResult validationResult = validator.Validate(model);
            validationResult.EnsureModelStateIsValid();
        }

        public static void EnsureModelStateIsValid(
            this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                validationResult.MapErrorsToViewModelAndThrows();
            }
        }

        public static void MapErrorsToViewModelAndThrows(
            this ValidationResult validationResult)
        {
            IEnumerable<ValidationError> errors = validationResult.Errors
                .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage));
            throw new ValidationErrorException(errors);

        }
    }
}
