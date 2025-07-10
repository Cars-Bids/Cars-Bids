using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Commands.Models;
public class CreateModelCommand : IRequest
{
    public int MakeId { get; set; }
    public string? Name { get; set; }

}

public class CreateModelsCommandHandler(
    IGenericRepository<Model> repository
    ) : IRequestHandler<CreateModelCommand>
{
    public async Task Handle(CreateModelCommand cmd, CancellationToken cancellationToken)
    {
        await repository.InsertAsync(new Model
        {
            MakeId = cmd.MakeId,
            Name = cmd.Name
        });
    }
}