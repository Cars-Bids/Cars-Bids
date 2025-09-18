using MediatR;
using Microsoft.AspNetCore.Http;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Chat;

public class SaveChatPhotosCommand : IRequest<List<string>>
{
    public List<IFormFile> Files { get; set; }
}

public class SaveChatPhotosCommandHandler(IFileService fileService) : IRequestHandler<SaveChatPhotosCommand, List<string>>
{
    public async Task<List<string>> Handle(SaveChatPhotosCommand request, CancellationToken cancellationToken)
    {
        var urls = await fileService.UploadImagesAsync(request.Files);
        return urls;
    }
}