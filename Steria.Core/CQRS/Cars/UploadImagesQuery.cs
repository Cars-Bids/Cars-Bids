using MediatR;
using Microsoft.AspNetCore.Http;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Cars;

public class UploadImagesQuery : IRequest<List<string>>
{
    public List<IFormFile> Files { get; set; }
}

public class UploadImagesQueryHandler(IFileService fileService) : IRequestHandler<UploadImagesQuery, List<string>>
{
    public async Task<List<string>> Handle(UploadImagesQuery request, CancellationToken cancellationToken)
    {
        return await fileService.UploadImagesAsync(request.Files);
    }
}