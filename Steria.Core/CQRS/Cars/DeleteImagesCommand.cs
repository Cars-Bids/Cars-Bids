using System.Net;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;

public class DeleteImagesCommand : IRequest
{
    public int CarId { get; set; }
    public int ImageId { get; set; }
}
public class DeleteImagesCommandHandler(IGenericRepository<CarImage> imageRepository,
                                        IFileService fileService) : IRequestHandler<DeleteImagesCommand>
{
    public async Task Handle(DeleteImagesCommand request, CancellationToken cancellationToken)
    {
        var image = await imageRepository.GetByIdAsync(request.ImageId)
                    ?? throw new HttpException($"Image {request.ImageId} not found", HttpStatusCode.NotFound);

        if (image.CarId != request.CarId)
            throw new HttpException("Image does not belong to this car", HttpStatusCode.BadRequest);

        var category = image.ImageCategory;
        var urlToDelete = image.ImageUrl;
        
        if (!string.IsNullOrEmpty(urlToDelete))
            await fileService.DeleteImagesByUrlsAsync(new List<string> { urlToDelete });
        
        await imageRepository.DeleteAsync(image.Id);
        
        var spec = new CarImagesByCategorySpec(request.CarId, category);
        var images = await imageRepository.GetListBySpec(spec, cancellationToken);

        var order = 1;
        foreach (var img in images)
        {
            img.OrderNumber = order++;
            await imageRepository.UpdateAsync(img);
        }
    }
}