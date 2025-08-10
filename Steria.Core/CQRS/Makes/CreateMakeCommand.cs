using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.Makes;
public class CreateMakeCommand : IRequest
{
    public string? Name { get; set; }
}

public class CreateMakesCommandHandler(
    IGenericRepository<Make> repository
    ) : IRequestHandler<CreateMakeCommand>
{
    public async Task Handle(CreateMakeCommand cmd, CancellationToken cancellationToken)
    {
        await repository.InsertAsync(new Make
        {
            Name = cmd.Name
        });
    }
}