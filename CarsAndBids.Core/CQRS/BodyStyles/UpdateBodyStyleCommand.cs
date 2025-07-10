using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.BodyStyles;

public class UpdateBodyStyleCommand : IRequest
{
    public int Id { get; set; }
    public string? StyleName { get; set; }
}

public class UpdateBodyStyleCommandHandler(
    IGenericRepository<BodyStyle> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateBodyStyleCommand>
{
    public async Task Handle(UpdateBodyStyleCommand cmd, CancellationToken cancellationToken)
    {
        var existingBodyStyle = await repository.GetByIdAsync(cmd.Id);

        mapper.Map(cmd, existingBodyStyle);

        await repository.UpdateAsync(existingBodyStyle!);
        return;
    }
}
