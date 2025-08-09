using FluentValidation;
using CarsAndBids.Core.CQRS.Makes;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Make;

public class CreateMakeCommandValidator : AbstractValidator<CreateMakeCommand>
{
    public CreateMakeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Resource.MakeNameRequired)
            .MaximumLength(50).WithMessage(Resource.MakeNameMaxLength);
    }
}
