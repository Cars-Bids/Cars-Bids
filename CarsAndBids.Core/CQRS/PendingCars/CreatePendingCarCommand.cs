using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarsAndBids.Core.CQRS.PendingCars;

public class CreatePendingCarCommand : IRequest<PendingCarDto>
{
    public required PendingCarDto PendingCar { get; set; }
}

public class CreatePendingCarCommandHandler(
    IGenericRepository<PendingCar> repository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : IRequestHandler<CreatePendingCarCommand, PendingCarDto>
{

    public async Task<PendingCarDto> Handle(CreatePendingCarCommand cmd, CancellationToken cancellationToken)
    {
        int ownerId = int.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var pendingCar = mapper.Map<PendingCar>(cmd.PendingCar);

        pendingCar.OwnerId = ownerId;
        pendingCar.CreatedAt = DateTime.UtcNow;

        await repository.InsertAsync(pendingCar);

        return mapper.Map<PendingCarDto>(pendingCar);
    }
}