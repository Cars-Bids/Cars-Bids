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
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository
    ) : IRequestHandler<GetAllCarsQuery, List<CarDto>>
{
    public async Task<List<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
        var cars = await carRepository.GetAsync();

        var allImages = await carImageRepository.GetAsync();

        var imagesByCarId = allImages
            .GroupBy(img => img.CarId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var carDtos = mapper.Map<List<CarDto>>(cars);
        var carImageDtos = mapper.Map<List<CarImageDto>>(allImages);


        foreach (var carDto in carDtos)
        {
            if (imagesByCarId.TryGetValue(carDto.Id, out var images))
            {
                carDto.Images = mapper.Map<List<CarImageDto>>(images);
            }
            else
            {
                carDto.Images = new List<CarImageDto>();
            }
        }

        return carDtos;
    }
}