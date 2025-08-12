using FluentValidation;
using Steria.Core.CQRS.NotificationTypes;

namespace Steria.Core.Validators.NotificationType;

public class UpdateNotificationTypeCommandValidator : AbstractValidator<UpdateNotificationTypeCommand>
{
    public UpdateNotificationTypeCommandValidator() //TODO: Add localization
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be at 50 characters long.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must be at 100 characters long.");
        
        RuleFor(x => x.RedirectRoute)
            .NotEmpty().WithMessage("RedirectRoute is required.")
            .MaximumLength(50).WithMessage("Route must be at 50 characters long.");

        RuleFor(x => x.SourceType)
            .IsInEnum().WithMessage("Invalid source type.");
        
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Notification type ID must be greater than 0");
    }
}