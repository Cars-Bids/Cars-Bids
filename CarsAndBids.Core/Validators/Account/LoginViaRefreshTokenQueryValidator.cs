using FluentValidation;
using CarsAndBids.Core.CQRS.Account;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Account;

public class LoginViaRefreshTokenQueryValidator : AbstractValidator<LoginViaRefreshTokenQuery>
{
    public LoginViaRefreshTokenQueryValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(Resource.RefreshTokenRequired);
    }
}
