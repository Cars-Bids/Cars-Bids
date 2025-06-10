using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.PendingCars;
public class DeletePendingCarByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeletePendingCarByIdHandler(
    IGenericRepository<PendingCar> repository
    ) : IRequestHandler<DeletePendingCarByIdCommand>
{
    public async Task Handle(DeletePendingCarByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}
