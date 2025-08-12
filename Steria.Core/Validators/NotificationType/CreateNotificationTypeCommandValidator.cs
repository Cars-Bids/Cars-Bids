using FluentValidation;
using Steria.Core.CQRS.NotificationTypes;

namespace Steria.Core.Validators.NotificationType;

public class CreateNotificationTypeCommandValidator : AbstractValidator<CreateNotificationTypeCommand>
{
    public CreateNotificationTypeCommandValidator() //TODO: Add localization
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
    }
}