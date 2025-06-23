using AutoMapper;
using CarsAndBids.Core.Exceptions;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Enums;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CarsAndBids.Core.CQRS.Cars;

public class UpdateCarCommand : IRequest
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string? Vin { get; set; }
    public string? Description { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string? Location { get; set; }
    public DrivetrainType Drivetrain { get; set; }
    public string? Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public CarStatus Status { get; set; }
    public int OwnerId { get; set; }
    public int AssingId { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public List<IFormFile>? NewImages { get; set; }
    public List<int>? ImagesToDelete { get; set; }
}

public class UpdateCarCommandHandler(
    IGenericRepository<Car> repository,
    IMapper mapper
    ) : IRequestHandler<UpdateCarCommand>
{
    public async Task Handle(UpdateCarCommand cmd, CancellationToken cancellationToken)
    {

        var existingCar = await repository.GetByIdAsync(cmd.Id)
            ?? throw new HttpException($"Car with id [{cmd.Id}] not found!", HttpStatusCode.NotFound);

        mapper.Map(cmd, existingCar);

        await repository.UpdateAsync(existingCar);
    }
}
