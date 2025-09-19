using System.Net;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;

public class UpdateCarImagesOrderCommand : IRequest
{
    public int CarId { get; set; }
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
        
        var spec = new CarImagesObjectByCarIdSpec(cmd.CarId);
        var images = await carImageRepo.GetListBySpec(spec, cancellationToken);
        
        var notBelonging = cmd.OrderedImageIds.Except(images.Select(i => i.Id)).ToList();
        if (notBelonging.Any())
            throw new HttpException($"Some images do not belong to car {cmd.CarId}", HttpStatusCode.BadRequest);
        
        int order = 1;
        foreach (var imageId in cmd.OrderedImageIds)
        {
            var img = images.First(i => i.Id == imageId);
            img.OrderNumber = order++;
            await carImageRepo.UpdateAsync(img);
        }
    }
}