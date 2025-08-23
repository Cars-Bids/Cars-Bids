using FluentValidation;
using Steria.Core.CQRS.Makes;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Make;

public class UpdateMakeCommandValidator : AbstractValidator<UpdateMakeCommand>
{
    public UpdateMakeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(Resource.MakeIdGreaterThanZero);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Resource.MakeNameRequired)
            .MaximumLength(50).WithMessage(Resource.MakeNameMaxLength);
    }
}
