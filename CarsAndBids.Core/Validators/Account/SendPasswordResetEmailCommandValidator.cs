using CarsAndBids.Core.CQRS.Account;
using CarsAndBids.Core.Resources;
using FluentValidation;

namespace CarsAndBids.Core.Validators.Account;

public class SendPasswordResetEmailCommandValidator : AbstractValidator<SendPasswordResetEmailCommand>
{
    public SendPasswordResetEmailCommandValidator()
    {
        RuleFor(x => x.MailTo)
            .NotEmpty().WithMessage(Resource.EmailRequired)
            .EmailAddress().WithMessage(Resource.EmailInvalid);
    }
}
