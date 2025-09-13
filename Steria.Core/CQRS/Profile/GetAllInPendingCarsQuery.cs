using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.Manager;
using MediatR;

namespace Steria.Core.CQRS.Manager;

public class GetAllInPendingCarsQuery : IRequest<PagedResult<ProfileInReviewCarDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetAllInPendingCarsQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetAllInPendingCarsHandler(
    IGenericRepository<Car> carRepository,
    IMapper mapper
    ) : IRequestHandler<GetAllInPendingCarsQuery, PagedResult<ProfileInReviewCarDto>>
{
    public async Task<PagedResult<ProfileInReviewCarDto>> Handle(GetAllInPendingCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new AllInPendingCarsSpec(request.PageNumber, request.PageSize);
        var cars = await carRepository.GetListBySpec(spec, cancellationToken);

        var profileInReviewCarDtos = mapper.Map<List<ProfileInReviewCarDto>>(cars);

        var countSpec = new AllInPendingCarsCountSpec();
        var totalCount = await carRepository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<ProfileInReviewCarDto>
        {
            Items = profileInReviewCarDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}