using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.BodyStyles;

public class UpdateBodyStyleCommand : IRequest<BodyStyleDto>
{
    public required BodyStyleDto BodyStyle { get; set; }
}

public class UpdateBodyStyleCommandHandler(
    IGenericRepository<BodyStyle> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateBodyStyleCommand, BodyStyleDto>
{
    public async Task<BodyStyleDto> Handle(UpdateBodyStyleCommand cmd, CancellationToken cancellationToken)
    {
        var existingBodyStyle = await repository.GetByIdAsync(cmd.BodyStyle.Id);

        mapper.Map(cmd.BodyStyle, existingBodyStyle);

        await repository.UpdateAsync(existingBodyStyle!);

        return mapper.Map<BodyStyleDto>(existingBodyStyle);
    }
}
