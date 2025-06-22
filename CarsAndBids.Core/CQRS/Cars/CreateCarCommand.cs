using AutoMapper;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Enums;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarsAndBids.Core.CQRS.Cars;

public class CreateCarCommand : IRequest
{
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
    public bool IsApproved { get; set; }
    public int BodyStyleId { get; set; }
    public int ModelId { get; set; }
    public List<IFormFile>? Images { get; set; }
}


public class CreateCarCommandHandler(
    IGenericRepository<Car> repository,
    IMapper mapper
    ) : IRequestHandler<CreateCarCommand>
{
    
    public async Task Handle(CreateCarCommand cmd, CancellationToken cancellationToken)
    {

        var car = mapper.Map<Car>(cmd);

        car.CreatedAt = DateTime.UtcNow;

        await repository.InsertAsync(car);
    }
}