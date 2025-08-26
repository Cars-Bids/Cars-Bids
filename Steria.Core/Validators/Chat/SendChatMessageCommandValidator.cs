using FluentValidation;
using Steria.Core.CQRS.Chat;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Chat;

public class SendChatMessageCommandValidator : AbstractValidator<SendChatMessageCommand>
{
    private const int MaxAttachments = 5;

    public SendChatMessageCommandValidator()
    {
        RuleFor(x => x.ChatId)
            .GreaterThan(0)
            .WithMessage(Resource.ChatIdGreaterThanZero);

        RuleFor(x => x.SenderId)
            .NotEmpty()
            .WithMessage(Resource.SenderIdRequired);

        RuleFor(x => x.Message)
            .NotEmpty()
            .When(x => x.Attachments is null || x.Attachments.Count == 0)
            .WithMessage(Resource.MessageOrAttachmentsRequired);

        RuleFor(x => x.Attachments)
            .Must(files => files == null || files.Count <= MaxAttachments)
            .WithMessage(string.Format(Resource.MaxAttachmentsAllowed, MaxAttachments));
    }
}