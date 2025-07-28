using FluentValidation;
using CarsAndBids.Core.CQRS.BodyStyles;

namespace CarsAndBids.Core.Validators.BodyStyle;

public class UpdateBodyStyleCommandValidator : AbstractValidator<UpdateBodyStyleCommand>
{
    public UpdateBodyStyleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Body style ID must be greater than 0");

        RuleFor(x => x.StyleName)
            .NotEmpty().WithMessage("Body style name is required")
            .MaximumLength(50).WithMessage("Body style name must be at most 50 characters long");
    }
}
