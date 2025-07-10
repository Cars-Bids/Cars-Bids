using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Commands.Makes;

public class DeleteMakeByIdCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteMakeByIdHandler(
    IGenericRepository<Make> repository
    ) : IRequestHandler<DeleteMakeByIdCommand>
{
    public async Task Handle(DeleteMakeByIdCommand cmd, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(cmd.Id);
    }
}
