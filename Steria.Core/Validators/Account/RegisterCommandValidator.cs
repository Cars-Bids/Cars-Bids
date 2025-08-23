using FluentValidation;
using Microsoft.AspNetCore.Http;
using Steria.Core.CQRS.Account;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Account;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Resource.UsernameRequired)
            .MinimumLength(4).WithMessage(Resource.UsernameMinLength)
            .MaximumLength(30).WithMessage(Resource.UsernameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Resource.EmailRequired)
            .EmailAddress().WithMessage(Resource.EmailInvalidFormat);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Resource.PasswordRequired)
            .MinimumLength(6).WithMessage(Resource.PasswordMinLength)
            .MaximumLength(20).WithMessage(Resource.PasswordMaxLength);

    }

    private bool BeValidImage(IFormFile? file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png" };
        return file is null || allowedTypes.Contains(file.ContentType);
    }
}
