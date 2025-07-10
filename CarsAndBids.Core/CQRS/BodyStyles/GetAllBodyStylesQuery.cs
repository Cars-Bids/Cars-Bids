using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.BodyStyles;

public class GetAllBodyStylesQuery : IRequest<List<BodyStyleDto>>
{
}

public class GetAllBodyStylesHandler(
    IMapper mapper,
    IGenericRepository<BodyStyle> repository
    ) : IRequestHandler<GetAllBodyStylesQuery, List<BodyStyleDto>>
{
    public async Task<List<BodyStyleDto>> Handle(GetAllBodyStylesQuery request, CancellationToken cancellationToken)
    {
        var bodyStyles = await repository.GetAsync();

        return mapper.Map<List<BodyStyleDto>>(bodyStyles);
    }
}
