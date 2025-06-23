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
    IGenericRepository<CarImage> carImageRepository
    ) : IRequestHandler<DeleteCarByIdCommand>
{
    public async Task Handle(DeleteCarByIdCommand cmd, CancellationToken cancellationToken)
    {
        var carImages = await carImageRepository.GetAsync(
            filter: ci => ci.CarId == cmd.Id);

        foreach (var carImage in carImages)
        {
            await carImageRepository.DeleteAsync(carImage.Id);
        }

        await carRepository.DeleteAsync(cmd.Id);
    }
}
