using AutoMapper;
using MediatR;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;

namespace Steria.Core.CQRS.BodyStyles;

public class GetBodyStyleByIdQuery : IRequest<BodyStyleDto?>
{
    public int Id { get; set; }
}

public class GetBodyStyleByIdHandler(
    IMapper mapper,
    IGenericRepository<BodyStyle> repository
    ) : IRequestHandler<GetBodyStyleByIdQuery, BodyStyleDto?>
{
    public async Task<BodyStyleDto?> Handle(GetBodyStyleByIdQuery request, CancellationToken cancellationToken)
    {
        var bodyStyle = await repository.GetByIdAsync(request.Id);

        return bodyStyle is null
            ? null
            : mapper.Map<BodyStyleDto>(bodyStyle);
    }
}
