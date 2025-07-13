using AutoMapper;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.BodyStyles;

public class CreateBodyStyleCommand : IRequest
{
    public string? StyleName { get; set; }
}

public class CreateBodyStyleCommandHandler(
    IMapper mapper,
    IGenericRepository<BodyStyle> repository
    ) : IRequestHandler<CreateBodyStyleCommand>
{
    public async Task Handle(CreateBodyStyleCommand cmd, CancellationToken cancellationToken)
    {
        var bodyStyle = mapper.Map<BodyStyle>(cmd);
        await repository.InsertAsync(bodyStyle);
    }
}