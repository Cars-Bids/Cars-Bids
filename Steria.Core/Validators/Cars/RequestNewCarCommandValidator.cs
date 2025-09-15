using System.Data;
using FluentValidation;
using Steria.Core.CQRS.Cars;
using Steria.Core.Enums;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Cars;

//TODO: add localization
public class RequestNewCarCommandValidator : AbstractValidator<RequestNewCarCommand>
{
    public RequestNewCarCommandValidator()
    {
        RuleFor(x => x.brandId)
            .NotEmpty().WithMessage("brandId is required.")
            .GreaterThan(0).WithMessage("brandId must be greater then 0.");

        RuleFor(x => x.fullName)
            .NotEmpty().WithMessage("Full name is required.");
        
        RuleFor(x => x.mileage)
            .NotEmpty().WithMessage("Mileage is required.")
            .GreaterThanOrEqualTo(0).WithMessage(Resource.MileageMin);

        RuleFor(x => x.modelId)
            .NotEmpty().WithMessage("ModelId is required.")
            .GreaterThan(0).WithMessage("ModelId must be greater then 0.");

        RuleFor(x => x.phone)
            .NotEmpty().WithMessage("Phone number is required.");

        RuleFor(x => x.photos)
            .NotEmpty().WithMessage("Photos are required.");

        RuleFor(x => (TransmissionType)x.transmissionId)
            .IsInEnum().WithMessage(Resource.TransmissionTypeInvalid);

        RuleFor(x => x.vin)
            .NotEmpty().WithMessage("Vin is required.");
        
        RuleFor(x => x.year)
            .NotEmpty().WithMessage("Year is required.")
            .GreaterThanOrEqualTo(1900).WithMessage(Resource.ManufactureYearMin)
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage(Resource.ManufactureYearMax);
    }
}