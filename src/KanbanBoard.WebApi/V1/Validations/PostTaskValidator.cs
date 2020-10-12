using System.Drawing;
using System.Globalization;
using FluentValidation;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class PostTaskValidator : ValidatorBase<PostTaskViewModel>
    {
        public PostTaskValidator()
        {
            RuleFor(task => task.Summary)
                .NotNull().WithMessage("Summary should not be null");
            RuleFor(task => task.Description)
                .NotNull().WithMessage("Description should not be null");
            RuleFor(task => task.TagColor)
                .Length(6).WithMessage("Tag color length must be 6")
                .Must(value => uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint _))
                .WithMessage("Tag color must be a valid hexadecimal color");
            RuleFor(task => task.AssignedTo)
                .NotNull().WithMessage("AssignedTo should not be null");
        }
    }
}
