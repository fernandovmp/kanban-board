using FluentValidation;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class SignUpValidator : ValidatorBase<SignUpViewModel>
    {
        public SignUpValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name is mandatory")
                .MaximumLength(100).WithMessage("Name length should be at a maximum of 100 characters");
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("E-mail is mandatory")
                .EmailAddress().WithMessage("Inform a valid e-mail");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is mandatory")
                .MinimumLength(8).WithMessage("Password length should be at a minimum of 8 characters");
            RuleFor(user => user.ConfirmPassword)
                .Equal(user => user.Password).WithMessage("Passwords doesn't match");
        }
    }
}
