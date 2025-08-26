using FluentValidation;
using Steria.Core.CQRS.Account;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Account;

public class SendPasswordResetEmailCommandValidator : AbstractValidator<SendPasswordResetEmailCommand>
{
    public SendPasswordResetEmailCommandValidator()
    {
        RuleFor(x => x.MailTo)
            .NotEmpty().WithMessage(Resource.EmailRequired)
            .EmailAddress().WithMessage(Resource.EmailInvalid);
    }
}
