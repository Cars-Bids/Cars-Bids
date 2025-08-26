using FluentValidation;
using Steria.Core.CQRS.Makes;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Make;

public class CreateMakeCommandValidator : AbstractValidator<CreateMakeCommand>
{
    public CreateMakeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Resource.MakeNameRequired)
            .MaximumLength(50).WithMessage(Resource.MakeNameMaxLength);
    }
}
