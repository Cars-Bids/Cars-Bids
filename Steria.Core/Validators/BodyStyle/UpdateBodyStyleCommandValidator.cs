using FluentValidation;
using Steria.Core.CQRS.BodyStyles;
using Steria.Core.Resources;

namespace Steria.Core.Validators.BodyStyle;

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
