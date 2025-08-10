using FluentValidation;
using Steria.Core.CQRS.Makes;

namespace Steria.Core.Validators.Make;

public class CreateMakeCommandValidator : AbstractValidator<CreateMakeCommand>
{
    public CreateMakeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Make name is required")
            .MaximumLength(50).WithMessage("Make name must be at most 50 characters long");
    }
}
