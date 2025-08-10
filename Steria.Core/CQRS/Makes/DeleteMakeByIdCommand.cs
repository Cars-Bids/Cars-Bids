using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Makes;

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
