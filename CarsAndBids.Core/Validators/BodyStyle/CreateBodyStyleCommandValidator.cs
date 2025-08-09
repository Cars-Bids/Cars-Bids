using FluentValidation;
using CarsAndBids.Core.CQRS.BodyStyles;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.BodyStyle;

public class CreateBodyStyleCommandValidator : AbstractValidator<CreateBodyStyleCommand>
{
    public CreateBodyStyleCommandValidator()
    {
        RuleFor(x => x.StyleName)
            .NotEmpty().WithMessage(Resource.BodyStyleNameRequired)
            .MaximumLength(50).WithMessage(Resource.BodyStyleNameMaxLength);
    
    }
}
