using Ardalis.Specification;
using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using CarsAndBids.Core.Specification.CarSpec;
using MediatR;

namespace CarsAndBids.Core.CQRS.Cars;

public class GetAllCarsQuery : IRequest<PagedResult<CarDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllCarsHandler(
    IMapper mapper,
    IGenericRepository<Car> carRepository
) : IRequestHandler<GetAllCarsQuery, PagedResult<CarDto>>
{
    public async Task<PagedResult<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
        var spec = new AllCarsWithImagesSpec(request.PageNumber, request.PageSize);
        var totalCount = await carRepository.CountAsync(new Specification<Car>(), cancellationToken);

        var cars = await carRepository.GetListBySpec(spec, cancellationToken);

        var dtoList = mapper.Map<List<CarDto>>(cars);

        return new PagedResult<CarDto>
        {
            Items = dtoList,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
