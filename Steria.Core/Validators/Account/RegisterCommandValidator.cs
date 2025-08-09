using FluentValidation;
using CarsAndBids.Core.CQRS.Account;
using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.Validators.Account;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
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

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(20).WithMessage("Password can't be longer than 20 characters");

        RuleFor(x => x.Image)
            .Must(BeValidImage)
            .WithMessage("Only JPEG or PNG images are allowed");
    }

    private bool BeValidImage(IFormFile? file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png" };
        return file is null || allowedTypes.Contains(file.ContentType);
    }
}
