using CarsAndBids.Core.Commands.Chat;
using FluentValidation;

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
            .When(x => x.Attachments == null || !x.Attachments.Any())
            .WithMessage("Message content or attachments are required.");

        RuleFor(x => x.Attachments)
            .Must(files => files == null || files.Count <= MaxAttachments)
            .WithMessage($"Maximum {MaxAttachments} attachments are allowed.");
    }
}