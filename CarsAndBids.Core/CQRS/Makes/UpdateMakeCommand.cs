using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Makes;

public class UpdateMakeCommand : IRequest<MakeDto>
{
    public required MakeDto Make { get; set; }
}

public class UpdateMakeCommandHandler(
    IGenericRepository<Make> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateMakeCommand, MakeDto>
{
    public async Task<MakeDto> Handle(UpdateMakeCommand cmd, CancellationToken cancellationToken)
    {
        var existingMake = await repository.GetByIdAsync(cmd.Make.Id);

        mapper.Map(cmd.Make, existingMake);

        await repository.UpdateAsync(existingMake!);

        return mapper.Map<MakeDto>(existingMake);
    }
}
