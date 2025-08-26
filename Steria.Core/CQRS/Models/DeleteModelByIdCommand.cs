using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Models;

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
