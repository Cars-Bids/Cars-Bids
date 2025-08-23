using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Resources;
using Steria.Core.Specification.CarSpec;

namespace Steria.Core.CQRS.Cars;
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
        var car = await carRepository.GetByIdAsync(cmd.Id)
            ?? throw new ArgumentException(string.Format(Resource.CarNotFoundById, cmd.Id));

        await fileService.DeleteImagesByUrlsAsync(
            await carImageRepository.GetListBySpec(new CarImagesByCarIdSpec(cmd.Id), cancellationToken));

        await carRepository.DeleteAsync(cmd.Id);
    }
}
