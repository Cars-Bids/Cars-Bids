using System.Net;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Enums;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;

public class UpdateCarImagesOrderCommand : IRequest
{
    public int CarId { get; set; }
    public ImageCategory Category { get; set; }
    public List<int> OrderedImageIds { get; set; } = [];
}
public class UpdateCarImagesOrderHandler(
    IGenericRepository<CarImage> carImageRepo
) : IRequestHandler<UpdateCarImagesOrderCommand>
{
    public async Task Handle(UpdateCarImagesOrderCommand cmd, CancellationToken cancellationToken)
    {
        if (cmd.OrderedImageIds == null || !cmd.OrderedImageIds.Any())
            throw new HttpException("OrderedImageIds cannot be empty", HttpStatusCode.BadRequest);
        
        var spec = new CarImagesByCategorySpec(cmd.CarId, cmd.Category);
        var images = await carImageRepo.GetListBySpec(spec, cancellationToken);
        
        if (images.Count != cmd.OrderedImageIds.Count)
            throw new HttpException("Mismatch between images count and provided order numbers", HttpStatusCode.BadRequest);
        
        var imagesByOrderNumber = images.ToDictionary(i => i.OrderNumber);

        for (int i = 0; i < cmd.OrderedImageIds.Count; i++)
        {
            var oldOrderNumber = cmd.OrderedImageIds[i];
            if (!imagesByOrderNumber.ContainsKey(oldOrderNumber))
                throw new HttpException($"Image with previous orderNumber {oldOrderNumber} not found", HttpStatusCode.BadRequest);

            imagesByOrderNumber[oldOrderNumber].OrderNumber = i + 1;
        }

        await carImageRepo.UpdateRangeAsync(images);
    }
}