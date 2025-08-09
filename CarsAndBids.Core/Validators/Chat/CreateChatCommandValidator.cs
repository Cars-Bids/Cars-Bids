using CarsAndBids.Core.CQRS.Chat;
using CarsAndBids.Core.Resources;
using FluentValidation;

namespace CarsAndBids.Core.Validators.Chat;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(x => x.ParticipantIds)
            .NotEmpty()
            .WithMessage(Resource.ParticipantsRequired);
    }
}
