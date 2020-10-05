using System.Globalization;
using FluentValidation;
using KanbanBoard.WebApi.V1.ViewModels;

namespace KanbanBoard.WebApi.V1.Validations
{
    public class PatchTaskValidator : ValidatorBase<PatchTaskViewModel>
    {
        public PatchTaskValidator()
        {

            RuleFor(task => task.TagColor)
                .Length(6).WithMessage("Tag color length must be 6")
                .Must(value => uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint _))
                .WithMessage("Tag color must be a valid hexadecimal color")
                .Unless(task => task.TagColor is null);
        }
    }
}
