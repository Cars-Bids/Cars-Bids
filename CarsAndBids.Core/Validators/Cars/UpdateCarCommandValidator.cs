using FluentValidation;
using CarsAndBids.Core.Commands.Cars;

namespace CarsAndBids.Core.Validators.Cars;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Id автомобіля є обов'язковим");

        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("Рік випуску є обов'язковим")
            .GreaterThanOrEqualTo(1900)
            .WithMessage("Рік випуску не може бути менше 1900")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("Рік випуску не може бути більше поточного");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Пробіг не може бути менше 0");

        RuleFor(x => x.Drivetrain)
            .IsInEnum()
            .WithMessage("Вказаний тип приводу не знайдено");

        RuleFor(x => x.TransmissionType)
            .IsInEnum()
            .WithMessage("Вказаний тип коробки передач не знайдено");

        RuleFor(x => x.Speeds)
            .GreaterThan(0)
            .WithMessage("Швидкість має бути більше 0");

        RuleFor(x => x.BodyStyleId)
            .GreaterThan(0)
            .WithMessage("Тип кузова є обов'язковим");

        RuleFor(x => x.ModelId)
            .GreaterThan(0)
            .WithMessage("Модель є обов'язковою");
    }
}
