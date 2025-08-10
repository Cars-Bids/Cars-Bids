using FluentValidation;
using Steria.Core.CQRS.Account;

namespace Steria.Core.Validators.Account;

public class LoginViaRefreshTokenQueryValidator : AbstractValidator<LoginViaRefreshTokenQuery>
{
    public LoginViaRefreshTokenQueryValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}
