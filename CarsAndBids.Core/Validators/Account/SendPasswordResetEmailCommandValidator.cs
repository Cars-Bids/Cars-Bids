using CarsAndBids.Core.CQRS.Account;
using FluentValidation;

namespace CarsAndBids.Core.Validators.Account;

public class SendPasswordResetEmailCommandValidator : AbstractValidator<SendPasswordResetEmailCommand>
{
    public SendPasswordResetEmailCommandValidator()
    {
        RuleFor(x => x.MailTo)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}
