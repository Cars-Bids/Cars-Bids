using FluentValidation;
using CarsAndBids.Core.CQRS.Account;

namespace CarsAndBids.Core.Validators.Account;

public class LoginViaRefreshTokenQueryValidator : AbstractValidator<LoginViaRefreshTokenQuery>
{
    public LoginViaRefreshTokenQueryValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
}
