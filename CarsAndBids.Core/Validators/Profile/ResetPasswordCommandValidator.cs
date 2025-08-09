using FluentValidation;
using CarsAndBids.Core.CQRS.Profile;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Profile;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
    .NotEmpty().WithMessage(Resource.EmailRequired)
    .EmailAddress().WithMessage(Resource.EmailInvalidFormat);

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage(Resource.ResetTokenRequired);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(Resource.NewPasswordRequired)
            .MinimumLength(6).WithMessage(Resource.NewPasswordMinLength)
            .MaximumLength(20).WithMessage(Resource.NewPasswordMaxLength);
    }
}
