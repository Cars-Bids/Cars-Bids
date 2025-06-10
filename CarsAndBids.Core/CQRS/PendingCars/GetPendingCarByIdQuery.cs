using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.PendingCars;

public class GetPendingCarByIdQuery : IRequest<PendingCarDto?>
{
    public int Id { get; set; }
}

public class GetPendingCarByIdHandler(
    IMapper mapper,
    IGenericRepository<PendingCar> repository
    ) : IRequestHandler<GetPendingCarByIdQuery, PendingCarDto?>
{
    public async Task<PendingCarDto?> Handle(GetPendingCarByIdQuery request, CancellationToken cancellationToken)
    {
        var pendingCar = await repository.GetByIdAsync(request.Id);

        return pendingCar is null
            ? null
            : mapper.Map<PendingCarDto>(pendingCar);
    }
}



