using FluentValidation;
using CarsAndBids.Core.CQRS.Chat;

namespace CarsAndBids.Core.Validators.Chat;

public class SendChatMessageCommandValidator : AbstractValidator<SendChatMessageCommand>
{
    private const int MaxAttachments = 5;

    public SendChatMessageCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .GreaterThan(0)
            .WithMessage("Chat ID must be greater than 0.");

        RuleFor(x => x.SenderId)
            .NotEmpty()
            .WithMessage("Sender ID is required.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .When(x => x.Attachments is not { Count: > 0 })
            .WithMessage("Message content or attachments are required.");

        RuleFor(x => x.Attachments)
            .Must(files => files is not { Count: > MaxAttachments })
            .WithMessage($"Maximum {MaxAttachments} attachments are allowed.");
    }
}