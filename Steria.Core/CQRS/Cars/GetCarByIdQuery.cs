using AutoMapper;
using MediatR;
using System.Net;
using Steria.Core.DTOs;
using Steria.Core.Entities;
using Steria.Core.Exceptions;
using Steria.Core.Interfaces;
using Steria.Core.Resources;

namespace Steria.Core.CQRS.Cars;

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
            ?? throw new HttpException(string.Format(Resource.CarNotFoundById, request.Id),HttpStatusCode.NotFound);

        var images = await carImageRepository.GetAsync(filter: img => img.CarId == request.Id);

        var carDto = mapper.Map<CarDto>(car);

        carDto.Images = mapper.Map<List<CarImageDto>>(images) ?? [];

        return carDto;
    }
}