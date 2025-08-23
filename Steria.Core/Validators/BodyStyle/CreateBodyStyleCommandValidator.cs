using FluentValidation;
using Steria.Core.CQRS.BodyStyles;
using Steria.Core.Resources;

namespace Steria.Core.Validators.BodyStyle;

public class CreateBodyStyleCommandValidator : AbstractValidator<CreateBodyStyleCommand>
{
    public CreateBodyStyleCommandValidator()
    {
        RuleFor(x => x.StyleName)
            .NotEmpty().WithMessage(Resource.BodyStyleNameRequired)
            .MaximumLength(50).WithMessage(Resource.BodyStyleNameMaxLength);
    
    }
}
