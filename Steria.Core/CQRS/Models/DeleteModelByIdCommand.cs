using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Models;

public class DeleteModelByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteModelByIdHandler(
    IGenericRepository<Model> repository
    ) : IRequestHandler<DeleteModelByIdCommand>
{
    public async Task Handle(DeleteModelByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}
