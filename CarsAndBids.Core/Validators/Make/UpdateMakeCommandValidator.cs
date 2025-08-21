using FluentValidation;
using CarsAndBids.Core.CQRS.Makes;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Make;

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
