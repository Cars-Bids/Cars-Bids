using FluentValidation;
using Steria.Core.CQRS.Account;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Account;

public class LoginViaRefreshTokenQueryValidator : AbstractValidator<LoginViaRefreshTokenQuery>
{
    public LoginViaRefreshTokenQueryValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(Resource.RefreshTokenRequired);
    }
}
