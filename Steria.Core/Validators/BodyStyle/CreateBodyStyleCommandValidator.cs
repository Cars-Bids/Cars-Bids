using FluentValidation;
using Steria.Core.CQRS.BodyStyles;

namespace Steria.Core.Validators.BodyStyle;

public class CreateBodyStyleCommandValidator : AbstractValidator<CreateBodyStyleCommand>
{
    public CreateBodyStyleCommandValidator()
    {
        RuleFor(x => x.StyleName)
            .NotEmpty().WithMessage("Body style name is required")
            .MaximumLength(50).WithMessage("Body style name must be at most 50 characters long");
    }
}
