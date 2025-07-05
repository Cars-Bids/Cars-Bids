using System.Linq.Expressions;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using CarsAndBids.Data.Persistence.Repositories.Specification;
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
        var filter = (Expression<Func<CarImage, bool>>)(ci => ci.CarId == cmd.Id);
        var selector = (Expression<Func<CarImage, string>>)(ci => ci.ImageUrl);
        var spec = new SelectByPropertySpec<CarImage, string>(filter, selector);
        var result = await carImageRepository.ListAsync(spec);
        var urls = (result as IEnumerable<string>)?.ToList() ?? new List<string>();

        await fileService.DeleteImagesByUrlsAsync(urls!);

        await carRepository.DeleteAsync(cmd.Id);
    }
}
