using FluentValidation;
using CarsAndBids.Core.CQRS.Models;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Model;

public class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
{
    public UpdateModelCommandValidator()
    {
        RuleFor(x => x.Id)
        .GreaterThan(0).WithMessage(Resource.ModelIdGreaterThanZero);

        RuleFor(x => x.MakeId)
            .GreaterThan(0).WithMessage(Resource.MakeIdGreaterThanZero);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Resource.ModelNameRequired)
            .MaximumLength(50).WithMessage(Resource.ModelNameMaxLength);
    }
}
