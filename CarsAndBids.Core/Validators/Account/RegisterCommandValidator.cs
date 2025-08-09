using FluentValidation;
using CarsAndBids.Core.CQRS.Account;
using Microsoft.AspNetCore.Http;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Account;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .WithMessage(Resource.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .WithMessage(Resource.LastNameMaxLength);

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

        RuleFor(x => x.Image)
            .Must(BeValidImage)
            .WithMessage(Resource.ImageInvalidFormat);
    }

    private bool BeValidImage(IFormFile? file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png" };
        return file is null || allowedTypes.Contains(file.ContentType);
    }
}
