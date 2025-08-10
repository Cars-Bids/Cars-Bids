using FluentValidation;
using Steria.Core.CQRS.Profile;

namespace Steria.Core.Validators.Profile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .WithMessage("First name can't be longer than 50 characters");

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .WithMessage("Last name can't be longer than 50 characters");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters")
            .MaximumLength(30).WithMessage("Username can't be longer than 30 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.ProfilePictureUrl)
            .MaximumLength(2048).WithMessage("Profile picture URL is too long")
            .Must(BeValidUrl).WithMessage("Profile picture URL must be a valid URL");
    }

    private bool BeValidUrl(string? url)
    {
        return string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }
}
