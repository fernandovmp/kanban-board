using FluentValidation;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class PostBoardMemberValidator : ValidatorBase<PostBoardMemberViewModel>
    {
        public PostBoardMemberValidator()
        {
            RuleFor(member => member.Email)
                .NotEmpty().WithMessage("Email is mandatory")
                .EmailAddress().WithMessage("Inform a valid e-mail");
            RuleFor(member => member.IsAdmin)
                .NotNull().WithMessage("Is Admin is mandatory");
        }
    }
}
