using FluentValidation;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class PostBoardValidator : ValidatorBase<PostBoardViewModel>
    {
        public PostBoardValidator()
        {
            RuleFor(board => board.Title)
                .NotEmpty().WithMessage("Title is mandatory")
                .MaximumLength(150).WithMessage("Title length should be at a maximum of 150 characters");
        }
    }
}
