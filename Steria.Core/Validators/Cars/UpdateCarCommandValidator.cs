using FluentValidation;
using Steria.Core.CQRS.Cars;

namespace Steria.Core.Validators.Cars;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("ID is required");

        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("Manufacture year is required")
            .GreaterThanOrEqualTo(1900)
            .WithMessage("Manufacture year can't be earlier than 1900")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("Manufacture year can't be greater than the current year");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Mileage can't be less than 0");

        RuleFor(x => x.Drivetrain)
            .IsInEnum()
            .WithMessage("Specified drivetrain type not found");

        RuleFor(x => x.TransmissionType)
            .IsInEnum()
            .WithMessage("Specified transmission type not found");

        RuleFor(x => x.Speeds)
            .GreaterThan(0)
            .WithMessage("Speed must be greater than 0");

        RuleFor(x => x.BodyStyleId)
            .GreaterThan(0)
            .WithMessage("Body type is required");

        RuleFor(x => x.ModelId)
            .GreaterThan(0)
            .WithMessage("Model is required");

        RuleFor(x => x.OwnerId)
            .GreaterThan(0)
            .WithMessage("Owner is required");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Specified car status not found");
    }
}
