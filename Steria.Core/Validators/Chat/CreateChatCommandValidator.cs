using FluentValidation;
using Steria.Core.CQRS.Chat;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Chat;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(x => x.ParticipantIds)
            .NotEmpty()
            .WithMessage(Resource.ParticipantsRequired);
    }
}
