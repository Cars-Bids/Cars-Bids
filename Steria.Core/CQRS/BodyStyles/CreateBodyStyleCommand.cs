using AutoMapper;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.BodyStyles;

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