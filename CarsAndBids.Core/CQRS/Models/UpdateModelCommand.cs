using AutoMapper;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Models;
public class UpdateModelCommand : IRequest
{
    public int Id { get; set; }
    public int MakeId { get; set; }
    public string? Name { get; set; }
}

public class UpdateModelCommandHandler(
    IGenericRepository<Model> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateModelCommand>
{
    public async Task Handle(UpdateModelCommand cmd, CancellationToken cancellationToken)
    {
        var existingModel = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingModel);

        await repository.UpdateAsync(existingModel!);

        return;
    }
}