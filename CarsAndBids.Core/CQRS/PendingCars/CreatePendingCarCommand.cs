using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Enums;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.PendingCars;

public class CreatePendingCarCommand : IRequest
{
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

public class CreatePendingCarCommandHandler(
    IGenericRepository<PendingCar> repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<CreatePendingCarCommand>
{

    public async Task Handle(CreatePendingCarCommand cmd, CancellationToken cancellationToken)
    {
        int ownerId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var pendingCar = mapper.Map<PendingCar>(cmd);

        pendingCar.OwnerId = ownerId;
        pendingCar.CreatedAt = DateTime.UtcNow;

        await repository.InsertAsync(pendingCar);

        return;
    }
}