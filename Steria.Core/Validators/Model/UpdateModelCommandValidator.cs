using FluentValidation;
using Steria.Core.CQRS.Models;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Model;

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
