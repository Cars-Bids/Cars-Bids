using FluentValidation;
using Steria.Core.CQRS.Chat;
using Steria.Core.Resources;

namespace Steria.Core.Validators.Chat;

public class SaveChatPhotosCommandValidator : AbstractValidator<SaveChatPhotosCommand>
{
    private const int MaxAttachments = 5;
    
    public SaveChatPhotosCommandValidator()
    {
        RuleFor(x => x.Files)
            .Must(files => files == null || files.Count <= MaxAttachments)
            .WithMessage(string.Format(Resource.MaxAttachmentsAllowed, MaxAttachments));
    }
}