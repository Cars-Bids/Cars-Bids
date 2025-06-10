using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Makes;

public class UpdateMakeCommand : IRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class UpdateMakeCommandHandler(
    IGenericRepository<Make> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateMakeCommand>
{
    public async Task Handle(UpdateMakeCommand cmd, CancellationToken cancellationToken)
    {
        var existingMake = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingMake);

        await repository.UpdateAsync(existingMake!);

        return;
    }
}
