using FluentValidation;
using CarsAndBids.Core.CQRS.BodyStyles;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.BodyStyle;

public class UpdateBodyStyleCommandValidator : AbstractValidator<UpdateBodyStyleCommand>
{
    public UpdateBodyStyleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(Resource.BodyStyleIdGreaterThanZero);

        RuleFor(x => x.StyleName)
            .NotEmpty().WithMessage(Resource.BodyStyleNameRequired)
            .MaximumLength(50).WithMessage(Resource.BodyStyleNameMaxLength);    
    }
}
