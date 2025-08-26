using AutoMapper;
using Steria.Core.DTOs;
using MediatR;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.BodyStyles;

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
    }
}
