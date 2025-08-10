using FluentValidation;
using Steria.Core.CQRS.Chat;

namespace Steria.Core.Validators.Chat;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(x => x.ParticipantIds)
            .NotEmpty()
            .WithMessage("Participants count must be greater than 0.");
    }
}
