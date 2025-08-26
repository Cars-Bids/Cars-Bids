using FluentValidation;
using Steria.Core.CQRS.Profile;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Profile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        //RuleFor(x => x.UserId)
        //    .GreaterThan(0).WithMessage(Resource.UserIdGreaterThanZero);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Resource.UsernameRequired)
            .MinimumLength(4).WithMessage(Resource.UsernameMinLength)
            .MaximumLength(30).WithMessage(Resource.UsernameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Resource.EmailRequired)
            .EmailAddress().WithMessage(Resource.EmailInvalidFormat);

        //RuleFor(x => x.ProfilePictureUrl)
        //    .MaximumLength(2048).WithMessage(Resource.ProfilePictureUrlMaxLength)
        //    .Must(BeValidUrl).WithMessage(Resource.ProfilePictureUrlInvalid);
    }

    private bool BeValidUrl(string? url)
    {
        return string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }
}
