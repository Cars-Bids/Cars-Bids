using AutoMapper;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Interfaces;
using Steria.Core.Specification.Manager;
using MediatR;

namespace Steria.Core.CQRS.Manager;

public class GetManagedCarsQuery : IRequest<PagedResult<AuctionWithCarDto>>
{
    public int UserId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public GetManagedCarsQuery(int userId, int pageNumber, int pageSize)
    {
        UserId = userId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetManagedCarsHandler(
    IGenericRepository<Car> carRepository,
    IMapper mapper
    ) : IRequestHandler<GetManagedCarsQuery, PagedResult<AuctionWithCarDto>>
{
    public async Task<PagedResult<AuctionWithCarDto>> Handle(GetManagedCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ManagedCarSpec(request.UserId, request.PageNumber, request.PageSize);
        var cars = await carRepository.GetListBySpec(spec, cancellationToken);

        var auctionWithCarDtos = mapper.Map<List<AuctionWithCarDto>>(cars);

        var countSpec = new ManagedCarCountSpec(request.UserId);
        var totalCount = await carRepository.CountAsync(countSpec, cancellationToken);

        return new PagedResult<AuctionWithCarDto>
        {
            Items = auctionWithCarDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}