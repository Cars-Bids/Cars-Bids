using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using System.Net;

namespace CarsAndBids.Core.CQRS.Cars;

public class GetCarByIdQuery : IRequest<CarDto>
{
    public int Id { get; set; }
}

public class GetCarByIdHandler(
    IMapper mapper,
    IGenericRepository<Car> repository
    ) : IRequestHandler<GetCarByIdQuery, CarDto>
{
    public async Task<CarDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
    {
        var car = await repository.GetByIdAsync(request.Id) 
            ?? throw new HttpException($"Car with id [{request.Id}] not found!", HttpStatusCode.NotFound);

        return mapper.Map<CarDto>(car);
    }
}



