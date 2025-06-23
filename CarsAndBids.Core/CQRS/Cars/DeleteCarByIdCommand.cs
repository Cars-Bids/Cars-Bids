using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Cars;
public class DeleteCarByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteCarByIdHandler(
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository,
    IFileService fileService
    ) : IRequestHandler<DeleteCarByIdCommand>
{
    public async Task Handle(DeleteCarByIdCommand cmd, CancellationToken cancellationToken)
    {
        var urls = (await carImageRepository.GetAsync(filter: ci => ci.CarId == cmd.Id))
            .Select(ci => ci.ImageUrl)
            .ToList();

        await fileService.DeleteImagesByUrlsAsync(urls!);

        await carRepository.DeleteAsync(cmd.Id);
    }
}
