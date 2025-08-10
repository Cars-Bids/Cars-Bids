using FluentValidation;
using Steria.Core.CQRS.Makes;

namespace Steria.Core.Validators.Make;

public class UpdateMakeCommandValidator : AbstractValidator<UpdateMakeCommand>
{
    public UpdateMakeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Make ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Make name is required")
            .MaximumLength(50).WithMessage("Make name must be at most 50 characters long");
    }
}
