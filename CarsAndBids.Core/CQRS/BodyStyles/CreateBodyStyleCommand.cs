using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.BodyStyles;

public class CreateBodyStyleCommand : IRequest
{
    public string? StyleName { get; set; }
}

public class CreateBodyStyleCommandHandler(
    IGenericRepository<BodyStyle> repository
    ) : IRequestHandler<CreateBodyStyleCommand>
{
    public async Task Handle(CreateBodyStyleCommand cmd, CancellationToken cancellationToken)
    {
        await repository.InsertAsync(new BodyStyle
        {
            StyleName = cmd.StyleName
        });
    }
}