using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.Cars;

public class CreateCarCommand : IRequest<CarDto>
{
    public required CarDto Car { get; set; }
    public List<IFormFile>? Images { get; set; }
}


public class CreateCarCommandHandler(
    IGenericRepository<Car> repository,
    IMapper mapper
    ) : IRequestHandler<CreateCarCommand, CarDto>
{
    
    public async Task<CarDto> Handle(CreateCarCommand cmd, CancellationToken cancellationToken)
    {

        var car = mapper.Map<Car>(cmd.Car);

        car.CreatedAt = DateTime.UtcNow;

        await repository.InsertAsync(car);

        return mapper.Map<CarDto>(car);
    }
}