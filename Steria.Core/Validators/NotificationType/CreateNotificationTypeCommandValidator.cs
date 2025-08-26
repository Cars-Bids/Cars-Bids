using FluentValidation;
using Steria.Core.CQRS.NotificationTypes;
using Steria.Core.Resources;

namespace Steria.Core.Validators.NotificationType;

public class CreateNotificationTypeCommandValidator : AbstractValidator<CreateNotificationTypeCommand>
{
    public CreateNotificationTypeCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage(Resource.KeyRequired)
            .MaximumLength(50).WithMessage(Resource.KeyMaxLength);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(Resource.DescriptionRequired)
            .MaximumLength(100).WithMessage(Resource.DescriptionMaxLength);
        
        RuleFor(x => x.RedirectRoute)
            .NotEmpty().WithMessage(Resource.RedirectRouteRequired)
            .MaximumLength(50).WithMessage(Resource.RedirectRouteMaxLength);

        RuleFor(x => x.SourceType)
            .IsInEnum().WithMessage(Resource.InvalidSourceType);
    }
}