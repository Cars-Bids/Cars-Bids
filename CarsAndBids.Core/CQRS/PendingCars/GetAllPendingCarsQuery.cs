using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Data.Entities;
using CarsAndBids.Data.Interfaces;
using MediatR;

namespace CarsAndBids.Core.CQRS.PendingCars;

public class GetAllPendingCarsQuery : IRequest<List<PendingCarDto>>
{
}

public class GetAllPendingCarsHandler(
    IMapper mapper,
    IGenericRepository<PendingCar> repository
    ) : IRequestHandler<GetAllPendingCarsQuery, List<PendingCarDto>>
{
    public async Task<List<PendingCarDto>> Handle(GetAllPendingCarsQuery request, CancellationToken cancellationToken)
    {
        var pendingCars = await repository.GetAsync();

        return mapper.Map<List<PendingCarDto>>(pendingCars);
    }
}