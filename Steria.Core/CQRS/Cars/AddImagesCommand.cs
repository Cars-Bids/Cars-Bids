using MediatR;
using Microsoft.AspNetCore.Http;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;

public class AddImagesCommand : IRequest<List<string>>
{
    public int CarId { get; set; }
    public List<IFormFile> Files { get; set; }
    public ImageCategory ImageCategory { get; set; }
}
public class AddImagesCommandHandler(IGenericRepository<CarImage> imageRepository,
                                     IFileService fileService) : IRequestHandler<AddImagesCommand, List<string>>
{
    public async Task<List<string>> Handle(AddImagesCommand request, CancellationToken cancellationToken)
    {
        var maxOrderSpec = new CarImagesByCategorySpec(request.CarId, request.ImageCategory);
        var existingImages = await imageRepository.GetListBySpec(maxOrderSpec, cancellationToken);
        
        var maxOrderNumber = existingImages.Any() 
            ? existingImages.Max(img => img.OrderNumber) + 1 
            : 1;
        
        var links = await fileService.UploadImagesAsync(request.Files);

        var images = links.Select(link => new CarImage
        {
            CarId = request.CarId,
            ImageCategory = request.ImageCategory,
            ImageUrl = link,
            OrderNumber = maxOrderNumber++
        });

        await imageRepository.InsertRangeAsync(images);

        return links;
    }
}