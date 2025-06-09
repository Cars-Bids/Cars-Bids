using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.Cars;

public class GetAllCarsQuery : IRequest<List<CarDto>>
{
}

public class GetAllCarsHandler(
    IMapper mapper,
    IGenericRepository<Car> repository
    ) : IRequestHandler<GetAllCarsQuery, List<CarDto>>
{
    public async Task<List<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
        var cars = await repository.GetAsync();

        return mapper.Map<List<CarDto>>(cars);
    }
}