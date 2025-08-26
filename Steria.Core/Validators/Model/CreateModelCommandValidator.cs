using FluentValidation;
using Steria.Core.CQRS.Models;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Model;

public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    public CreateModelCommandValidator()
    {
        RuleFor(x => x.MakeId)
            .GreaterThan(0).WithMessage(Resource.MakeIdGreaterThanZero);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Resource.ModelNameRequired)
            .MaximumLength(50).WithMessage(Resource.ModelNameMaxLength);
    }
}
