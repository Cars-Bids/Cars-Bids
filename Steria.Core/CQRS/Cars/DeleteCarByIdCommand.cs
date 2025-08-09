using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Specification.CarSpec;
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
        var car = await carRepository.GetByIdAsync(cmd.Id);
        if (car == null)
            throw new ArgumentException($"Car with ID {cmd.Id} not found.");

        await fileService.DeleteImagesByUrlsAsync(
            await carImageRepository.GetListBySpec(new CarImagesByCarIdSpec(cmd.Id), cancellationToken));

        await carRepository.DeleteAsync(cmd.Id);
    }
}
