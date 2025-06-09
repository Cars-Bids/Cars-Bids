using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Cars;
public class DeleteCarByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteCarByIdHandler(
    IGenericRepository<Car> repository
    ) : IRequestHandler<DeleteCarByIdCommand>
{
    public async Task Handle(DeleteCarByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}
