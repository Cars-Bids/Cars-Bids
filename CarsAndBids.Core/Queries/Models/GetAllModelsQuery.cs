using AutoMapper;
using CarsAndBids.Core.DTOs;
using CarsAndBids.Core.Entities;
using CarsAndBids.Core.Interfaces;
using MediatR;

namespace CarsAndBids.Core.Queries.Models;
public class GetAllModelsQuery : IRequest<List<ModelDto>>
{
}

public class GetAllModelsHandler(
    IMapper mapper,
    IGenericRepository<Model> repository
    ) : IRequestHandler<GetAllModelsQuery, List<ModelDto>>
{
    public async Task<List<ModelDto>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        var model = await repository.GetAsync();

        return mapper.Map<List<ModelDto>>(model);
    }
}
