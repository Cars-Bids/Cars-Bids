using Ardalis.Specification;
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.BodyStyleSpec;
using MediatR;


namespace CarsAndBids.Core.CQRS.BodyStyles;

public class GetAllBodyStylesQuery : IRequest<PagedResult<BodyStyleDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllBodyStylesHandler(
    IMapper mapper,
    IGenericRepository<BodyStyle> repository
) : IRequestHandler<GetAllBodyStylesQuery, PagedResult<BodyStyleDto>>
{
    public async Task<PagedResult<BodyStyleDto>> Handle(GetAllBodyStylesQuery request, CancellationToken cancellationToken)
    {
        var spec = new PagedBodyStylesSpec(request.PageNumber, request.PageSize);

        var totalCount = await repository.CountAsync(new Specification<BodyStyle>(), cancellationToken);
        var bodyStyles = await repository.GetListBySpec(spec, cancellationToken);

        var dtoList = mapper.Map<List<BodyStyleDto>>(bodyStyles);

        return new PagedResult<BodyStyleDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
