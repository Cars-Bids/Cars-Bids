using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Queries.Makes;

public class GetAllMakesQuery : IRequest<List<MakeDto>>
{
}

public class GetAllMakesHandler(
    IMapper mapper,
    IGenericRepository<Make> repository
    ) : IRequestHandler<GetAllMakesQuery, List<MakeDto>>
{
    public async Task<List<MakeDto>> Handle(GetAllMakesQuery request, CancellationToken cancellationToken)
    {
        var make = await repository.GetAsync();

        return mapper.Map<List<MakeDto>>(make);
    }
}
