using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Enums;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.PendingCars;

public class UpdatePendingCarCommand : IRequest
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string? Vin { get; set; }
    public string? ExteriorColor { get; set; }
    public string? InteriorColor { get; set; }
    public int Mileage { get; set; }
    public string? Location { get; set; }
    public DrivetrainType Drivetrain { get; set; }
    public string? Engine { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public int Speeds { get; set; }
    public string? Modifications { get; set; }
    public string? Flaws { get; set; }
    public int ModelId { get; set; }
    public int BodyStyleId { get; set; }
}

public class UpdatePendingCarCommandHandler(
    IGenericRepository<PendingCar> repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<UpdatePendingCarCommand>
{
    public async Task Handle(UpdatePendingCarCommand cmd, CancellationToken cancellationToken)
    {
        int ownerId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var existingPendingCar = await repository.GetByIdAsync(cmd.Id);

        if (existingPendingCar == null)
        {
            throw new KeyNotFoundException($"Pending car with ID {cmd.Id} not found.");
        }

        if (existingPendingCar.OwnerId != ownerId)
        {
            throw new UnauthorizedAccessException("You are not the owner of this car.");
        }

        mapper.Map(cmd, existingPendingCar);

        await repository.UpdateAsync(existingPendingCar);

        return;
        
    }
}