using MediatR;
using Microsoft.AspNetCore.Http;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Interfaces;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;

public class AddImagesCommand : IRequest
{
    public int CarId { get; set; }
    public List<IFormFile> Files { get; set; }
    public ImageCategory ImageCategory { get; set; }
}
public class AddImagesCommandHandler(IGenericRepository<CarImage> imageRepository,
                                     IFileService fileService) : IRequestHandler<AddImagesCommand>
{
    public async Task Handle(AddImagesCommand request, CancellationToken cancellationToken)
    {
        var maxOrderSpec = new CarImagesMaxOrderNumberSpec(request.CarId);
        int maxOrderNumber = (await imageRepository.GetListBySpec(maxOrderSpec, cancellationToken))
            .DefaultIfEmpty(0)
            .Max() + 1;

        var links = await fileService.UploadImagesAsync(request.Files);
        var images = links.Select(x => new CarImage
        {
            CarId = request.CarId,
            ImageCategory = request.ImageCategory,
            ImageUrl = x
        });

        await imageRepository.InsertRangeAsync(images);
    }
}