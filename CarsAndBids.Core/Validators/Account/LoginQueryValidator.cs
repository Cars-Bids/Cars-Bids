using FluentValidation;
using CarsAndBids.Core.CQRS.Account;
using CarsAndBids.Core.Resources;

namespace CarsAndBids.Core.Validators.Account;
public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email)
    .NotEmpty().WithMessage(Resource.EmailRequired)
    .EmailAddress().WithMessage(Resource.InvalidEmailFormat);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Resource.PasswordRequired)
            .MinimumLength(6).WithMessage(Resource.PasswordMinLength)
            .MaximumLength(20).WithMessage(Resource.PasswordMaxLength);
    }
}
