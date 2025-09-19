using FluentValidation;
using Steria.Core.CQRS.Cars;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Cars;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Resource.IdRequired)
            .GreaterThan(0).WithMessage(Resource.IdRequired);

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage(Resource.ManufactureYearRequired)
            .GreaterThanOrEqualTo(1900).WithMessage(Resource.ManufactureYearMin)
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage(Resource.ManufactureYearMax);

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage(Resource.MileageMin);
        
        RuleFor(x => x.Speeds)
            .GreaterThan(0).WithMessage(Resource.SpeedsGreaterThanZero);

        RuleFor(x => x.BodyStyleId)
            .GreaterThan(0).WithMessage(Resource.BodyStyleIdRequired);

        RuleFor(x => x.ModelId)
            .GreaterThan(0).WithMessage(Resource.ModelIdRequired);
        
    }
}
