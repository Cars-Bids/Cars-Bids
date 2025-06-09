using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.Cars;

public class UpdateCarCommand : IRequest<CarDto>
{
    public required CarDto Car { get; set; }
    public List<IFormFile>? NewImages { get; set; }
    public List<int>? ImagesToDelete { get; set; }
}

public class UpdateCarCommandHandler(
    IGenericRepository<Car> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateCarCommand, CarDto>
{
    public async Task<CarDto> Handle(UpdateCarCommand cmd, CancellationToken cancellationToken)
    {

        var existingCar = await repository.GetByIdAsync(cmd.Car.Id);

        mapper.Map(cmd.Car, existingCar);

        await repository.UpdateAsync(existingCar!);

        return mapper.Map<CarDto>(existingCar);
    }
}
