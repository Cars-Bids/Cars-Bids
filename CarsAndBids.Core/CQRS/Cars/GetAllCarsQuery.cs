using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using CarsAndBids.Data.Persistence.Repositories.Specification.CarSpec;
using MediatR;

namespace CarsAndBids.Core.CQRS.Cars;

public class GetAllCarsQuery : IRequest<List<CarDto>>
{
}

public class GetAllCarsHandler(
    IMapper mapper,
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository
) : IRequestHandler<GetAllCarsQuery, List<CarDto>>
{
    public async Task<List<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
        var cars = await carRepository.GetListBySpec(new AllCarsWithImagesSpec(), cancellationToken);
        return mapper.Map<List<CarDto>>(cars);
    }
}