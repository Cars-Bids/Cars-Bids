using CarsAndBids.Core.CQRS.Chat;
using FluentValidation;

namespace CarsAndBids.Core.Validators.Chat;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(x => x.ParticipantIds)
            .NotEmpty()
            .WithMessage("Participants count must be greater than 0.");
    }
}
