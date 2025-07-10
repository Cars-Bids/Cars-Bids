using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;
using System.Net;

namespace CarsAndBids.Core.Queries.Cars;

public class GetCarByIdQuery : IRequest<CarDto>
{
    public int Id { get; set; }
}

public class GetCarByIdHandler(
    IMapper mapper,
    IGenericRepository<Car> carRepository,
    IGenericRepository<CarImage> carImageRepository
    ) : IRequestHandler<GetCarByIdQuery, CarDto>
{
    public async Task<CarDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
    {
        var car = await carRepository.GetByIdAsync(request.Id)
            ?? throw new HttpException($"Car with id [{request.Id}] not found!", HttpStatusCode.NotFound);

        var images = await carImageRepository.GetAsync(filter: img => img.CarId == request.Id);

        var carDto = mapper.Map<CarDto>(car);

        carDto.Images = mapper.Map<List<CarImageDto>>(images) ?? new List<CarImageDto>();

        return carDto;
    }
}