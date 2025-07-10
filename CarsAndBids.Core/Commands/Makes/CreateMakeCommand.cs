using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Commands.Makes;
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