using FluentValidation;
using CarsAndBids.Core.CQRS.Models;

namespace CarsAndBids.Core.Validators.Model;

public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    public CreateModelCommandValidator()
    {
        RuleFor(x => x.MakeId)
            .GreaterThan(0).WithMessage("Make ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Model name is required")
            .MaximumLength(50).WithMessage("Model name must be at most 50 characters long");
    }
}
