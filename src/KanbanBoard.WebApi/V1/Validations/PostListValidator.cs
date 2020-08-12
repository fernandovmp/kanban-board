using FluentValidation;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class PostListValidator : ValidatorBase<PostListViewModel>
    {
        public PostListValidator()
        {
            RuleFor(list => list.Title)
                .NotEmpty().WithMessage("Title is mandatory")
                .MaximumLength(150).WithMessage("Title length should be at a maximum of 150 characters");
        }
    }
}
