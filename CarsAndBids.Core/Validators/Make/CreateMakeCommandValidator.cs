using FluentValidation;
using CarsAndBids.Core.CQRS.Makes;

namespace CarsAndBids.Core.Validators.Make;

public class CreateMakeCommandValidator : AbstractValidator<CreateMakeCommand>
{
    public CreateMakeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Make name is required")
            .MaximumLength(50).WithMessage("Make name must be at most 50 characters long");
    }
}
